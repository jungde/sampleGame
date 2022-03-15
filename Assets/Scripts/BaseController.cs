using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    protected Define.CharacterType _characterType;

    protected SpriteRenderer _spriterender;
    protected Rigidbody2D _rigid2D;
    protected CircleCollider2D _circleCollider2D;
    protected Animator _animator;

    protected Vector2 _direction; // 이동방향
    protected Vector2 _predirection;
    protected Define.State _state = Define.State.Idle;

    public Vector2 Direction
    {
        get { return _direction; }
        set
        {
            _direction = value;

            if (_state == Define.State.Die) return;
            if (_direction.x == 0.0f && _direction.y == 0.0f)
            {
                State = Define.State.Idle;
            }
            else
            {
                State = Define.State.Moving;                
            }

            _predirection = _direction;
        }
    }

    public Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            switch(_characterType)
            {
                case Define.CharacterType.Player:
                    {
                        PlayerState();
                    }
                    break;

                case Define.CharacterType.Monster:
                    {
                        MonsterState();
                    }
                    break;
            }

            
        }
    }

    void Start()
    {

        Init();
    }

    
    protected virtual void Update()
    {
        
    }

    protected virtual void LateUpdate()
    {
        
    }

    void PlayerState()
    {
        switch (_state)
        {
            case Define.State.Idle:
                {
                    _animator.speed = 1.0f;

                    if (_predirection.x < 0.0f)
                    {
                        _animator.CrossFade("idle_side", 0.0f);
                        _spriterender.flipX = false;
                    }
                    else if (_predirection.x > 0.0f)
                    {
                        _animator.CrossFade("idle_side", 0.0f);
                        _spriterender.flipX = true;
                    }

                    if (_predirection.y < 0.0f)
                        _animator.CrossFade("idle_down", 0.0f);
                    else if (_predirection.y > 0.0f)
                        _animator.CrossFade("idle_up", 0.0f);
                }
                break;
            case Define.State.Moving:
                {
                    _animator.speed = 1.0f;

                    if (_direction.x < 0.0f)
                    {
                        _animator.CrossFade("run_side", 0.0f);
                        _spriterender.flipX = false;
                    }
                    else if (_direction.x > 0.0f)
                    {
                        _animator.CrossFade("run_side", 0.0f);
                        _spriterender.flipX = true;
                    }

                    if (_direction.y < 0.0f)
                        _animator.CrossFade("run_down", 0.0f);
                    else if (_direction.y > 0.0f)
                        _animator.CrossFade("run_up", 0.0f);
                }
                break;
            case Define.State.Die:
                _animator.speed = 0.0f;
                break;
        }
    }

    void MonsterState()
    {
        switch (_state)
        {
            case Define.State.Idle:
                {
                    
                }
                break;
            case Define.State.Moving:
                {
                    
                }
                break;
            case Define.State.Die:
                break;
        }
    }


    protected abstract void Init();
}
