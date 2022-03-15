using System;
using System.Collections.Generic;
using UnityEngine;

public class MonsterController : BaseController
{
    Scene_InGame _game;

    float _speed = 0.01f;
    Vector3 _dest;

    enum LookDir
    {
        Up = 0,
        Left = 1,
        Down = 2,
        Right = 3,
    }
    int _lookDir = (int)LookDir.Up;

    protected override void Init()
    {
        _characterType = Define.CharacterType.Monster;

        _spriterender = gameObject.GetComponentInChildren<SpriteRenderer>();
        _rigid2D = GetComponent<Rigidbody2D>();
        _circleCollider2D = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _game = GameObject.Find("@Scene").GetComponent<Scene_InGame>();

        _dest = transform.position;
    }

    protected override void Update()
    {
        if( (_game.GetCellIndex(transform.position) == _game.GetCellIndex(_dest)) &&
            Vector3.Magnitude(_dest - transform.position)<=0.05f )
        {
            transform.position = _dest;
            FindingWay();
        }

        transform.position = Vector3.MoveTowards(transform.position, _dest, _speed);
    }

    struct WayData
    {
        public int lookDir;
        public Vector3 vecDir;
    }
    void FindingWay()
    {
        Vector3[,] searchDir = new Vector3[,] { 
            { new Vector3( 0, 1, 0), new Vector3(-1, 0, 0), new Vector3( 0,-1, 0), new Vector3( 1, 0, 0) },
            { new Vector3(-1, 0, 0), new Vector3( 0,-1, 0), new Vector3( 1, 0, 0), new Vector3( 0, 1, 0) },
            { new Vector3( 0,-1, 0), new Vector3( 1, 0, 0), new Vector3( 0, 1, 0), new Vector3(-1, 0, 0) },
            { new Vector3( 1, 0, 0), new Vector3( 0, 1, 0), new Vector3(-1, 0, 0), new Vector3( 0,-1, 0) }};


        Vector3Int curcell = _game.GetCellIndex(transform.position);
        List<WayData> possible = new List<WayData>();
        for (int i = 0; i < 4; i++)
        {
            Vector3Int v = new Vector3Int((int)searchDir[_lookDir, i].x, (int)searchDir[_lookDir, i].y, (int)searchDir[_lookDir, i].z);
            if (_game._mapData[curcell.y + v.y, curcell.x + v.x] == Scene_InGame.MapTile.Floor)
            {
                WayData data = new WayData();
                data.lookDir = i;
                data.vecDir = searchDir[_lookDir, i];
                possible.Add(data);
            }                
        }

        
        System.Random rand = new System.Random((int)DateTime.Now.Ticks);
        int randNum = rand.Next(0, possible.Count);
        _lookDir = possible[randNum].lookDir;
        Direction = CalcDirection((LookDir)_lookDir);
        _dest = _game.GetCellCenterWorld(
                new Vector3Int(curcell.x + (int)possible[randNum].vecDir.x, curcell.y + (int)possible[randNum].vecDir.y, 0));
    }

    Vector2 CalcDirection(LookDir dir)
    {
        Vector2 v = Vector2.zero;
        switch (dir)
        {
            case LookDir.Up:
                v.x = 0.0f;
                v.y = 1.0f;
                break;
            case LookDir.Left:
                v.x = -1.0f;
                v.y = 0.0f;
                break;
            case LookDir.Down:
                v.x = 0.0f;
                v.y = -1.0f;
                break;
            case LookDir.Right:
                v.x = 1.0f;
                v.y = 0.0f;
                break;
        }
        return v;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BombExplosion")
            Destroy(gameObject);
    }
}
