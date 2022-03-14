using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : BaseController
{
	[SerializeField]
	Transform _playerStartPosition = default;
	Vector3Int _posCellPlayer;

	AudioSource _audioSourceRun;
	AudioClip _audioClipRun;

	GameObject _bomb;

	[SerializeField]
	float speed = 5.0f;

	[SerializeField]
	Tilemap _tilemapTerrain = default;

	Vector2 _camerasight;
	Vector2 _startpoint;
	Vector2 _endpoint;

	protected override void Init()
	{
		_characterType = Define.CharacterType.Player;

		_spriterender = GetComponent<SpriteRenderer>();
		_rigid2D = GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator>();

		Vector3Int posStart = _tilemapTerrain.WorldToCell(_playerStartPosition.position);
		Vector3 posCellCenter = _tilemapTerrain.GetCellCenterLocal(posStart);
		transform.position = posCellCenter - new Vector3(0.0f, 0.5f, 0.0f);

		_audioClipRun = Resources.Load<AudioClip>("Hit23");

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


		_bomb = Resources.Load<GameObject>("Prefabs/Bomb");
	}

	protected override void Update()
    {
		_posCellPlayer = _tilemapTerrain.WorldToCell(transform.position + new Vector3(0.0f, 0.5f, 0.0f));

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
		Instantiate(_bomb, _tilemapTerrain.GetCellCenterLocal(_posCellPlayer), Quaternion.identity);
	}
}