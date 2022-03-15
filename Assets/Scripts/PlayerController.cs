using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : BaseController
{
	AudioSource _audioSourceRun;
	AudioClip _audioClipRun;
	GameObject _bomb;

	Scene_InGame _game;
	Vector2 _camerasight;
	Vector2 _startpoint;
	Vector2 _endpoint;
	float speed = 5.0f;

    struct Buff
    {
		public bool _Inv;
    }
	Buff _buff;

	protected override void Init()
	{
		_characterType = Define.CharacterType.Player;

		_spriterender = GetComponent<SpriteRenderer>();
		_rigid2D = GetComponent<Rigidbody2D>();
		_circleCollider2D = GetComponent<CircleCollider2D>();
		_animator = GetComponent<Animator>();
		_audioClipRun = Resources.Load<AudioClip>("Hit23");
		_game = GameObject.Find("@Scene").GetComponent<Scene_InGame>();
		_bomb = Resources.Load<GameObject>("Prefabs/Bomb");

		float h = Camera.main.GetComponent<Camera>().orthographicSize;
		float aspect = (float)Screen.width / (float)Screen.height;
		_camerasight.x = (aspect * h) * 2f;
		_camerasight.y = h * 2f;
		_startpoint.x = aspect * h;
		_startpoint.y = h;
		_endpoint.x = (_camerasight.x * 0.5f) + 29.0f - _camerasight.x;
		_endpoint.y = (_camerasight.y * 0.5f) + 13.0f - _camerasight.y;

		_audioSourceRun = gameObject.AddComponent<AudioSource>();
		_audioSourceRun.pitch = 1.0f;
		_audioSourceRun.clip = _audioClipRun;

		GameObject.Find("MobileInput").GetComponent<Input_ByMobile>()._player = this;
		GameObject.Find("PCInput").GetComponent<Input_ByPC>()._player = this;

		ResetPlayer();
	}

	protected override void Update()
    {
		switch (State)
		{
			case Define.State.Idle:
				_audioSourceRun.Stop();
				break;
			case Define.State.Moving:
				if(!_audioSourceRun.isPlaying)
					_audioSourceRun.Play();
				break;
			case Define.State.Die:
				
				break;
		}

		_rigid2D.velocity =  Direction * speed;
    }

	protected override void LateUpdate()
	{
		Camera.main.transform.localPosition = new Vector3(0.0f, 0.0f, Camera.main.transform.localPosition.z);

		if (Camera.main.transform.position.x < _startpoint.x ||
            Camera.main.transform.position.x > _endpoint.x   ||
            Camera.main.transform.position.y < _startpoint.y ||
			Camera.main.transform.position.y > _endpoint.y)
        {
			Vector3 pos = Camera.main.transform.position;
			pos.x = Mathf.Clamp(pos.x, _startpoint.x, _endpoint.x);
			pos.y = Mathf.Clamp(pos.y, _startpoint.y, _endpoint.y);
			Camera.main.transform.position = pos;
		}
    }

	void Blink()
	{
		_buff._Inv = true;

		Color color = _spriterender.color;
		if (color.a == 1.0f)
			color.a = 0.0f;
		else
			color.a = 1.0f;
		_spriterender.color = color;
	}

	void CancelBlink()
    {
		_buff._Inv = false;
		Color color = _spriterender.color;
		color.a = 1.0f;
		_spriterender.color = color;

		CancelInvoke("Blink");
    }

	void Dying()
    {
		_circleCollider2D.enabled = false;
		Color color = _spriterender.color;
		color.a -= 0.05f;
		_spriterender.color = color;
		if (color.a <= 0.0f)
        {
			ResetPlayer();
			CancelInvoke("Dying");
		}
	}

	void ResetPlayer()
    {
		Vector3Int posStart = _game._tilemapFloor.WorldToCell(_game._startPoint.transform.position);
		Vector3 posCellCenter = _game._tilemapFloor.GetCellCenterLocal(posStart) - new Vector3(0.0f, 0.5f, 0.0f);
		transform.position = posCellCenter;

		_circleCollider2D.enabled = true;

		_state = Define.State.Idle;
		NoMove();

		InvokeRepeating("Blink", 0f, 0.05f);
		Invoke("CancelBlink", 2.0f);
	}

	public void SetDie()
    {
		InvokeRepeating("Dying", 0f, 0.1f);
		State = Define.State.Die;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//Debug.Log("=============OnTriggerEnter2D");

		if (collision.tag == "Enemy" || collision.tag == "BombExplosion")
		{
			if (_buff._Inv) return;

			SetDie();

		}
	}

	public void LeftMove()
    {
		Vector2 v = Direction;
		v.x = -1.0f;
		Direction = v;
	}

    public void RightMove()
    {
		Vector2 v = Direction;
		v.x = 1.0f;
		Direction = v;
	}

    public void UpMove()
    {
		Vector2 v = Direction;
		v.y = 1.0f;
		Direction = v;
	}

    public void DownMove()
    {
		Vector2 v = Direction;
		v.y = -1.0f;
		Direction = v;
	}

	public void NoMove()
    {
		Vector2 v = Direction;
		v = Vector2.zero;
		Direction = v;
	}

    public void SetupBomb()
    {
		Vector3Int cell = _game._tilemapFloor.WorldToCell(transform.position + new Vector3(0.0f, 0.5f, 0.0f));
		Instantiate(_bomb, _game._tilemapFloor.GetCellCenterLocal(cell), Quaternion.identity);
	}


}