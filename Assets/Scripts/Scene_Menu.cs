using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Scene_Menu : MonoBehaviour
{
    [SerializeField]
    GameObject StartText = default;

    Grid _grid;
    Tilemap _tilemapHill;
    Tilemap _tilemapWall;
    Tilemap _tilemapFloor;

    Vector3 _targetCamera;
    int _mapFloorWidth = 29;
    int _mapFloorHeight = 13;
    public Define.MapTile[,] _mapData = default;

    private void Awake()
    {
        _grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        _tilemapHill = GameObject.Find("Tilemap_Hill").GetComponent<Tilemap>();
        _tilemapWall = GameObject.Find("Tilemap_Wall").GetComponent<Tilemap>();
        _tilemapFloor = GameObject.Find("Tilemap_Floor").GetComponent<Tilemap>();

        InvokeRepeating("Blink", 0f, 0.5f);

        _mapData = new Define.MapTile[_mapFloorHeight, _mapFloorWidth];
        InitMapData();

        GameObject enemy = Resources.Load<GameObject>("Prefabs/blue1");
        GameObject[] gos = GameObject.FindGameObjectsWithTag("SpawnPoint");
        List<GameObject> lgos = new List<GameObject>(gos);
        foreach (GameObject o in lgos)
        {
            Vector3Int cellIndex = _grid.WorldToCell(o.transform.position);
            Vector3 cellCenterWorld = _grid.GetCellCenterLocal(cellIndex);

            Instantiate(enemy, cellCenterWorld, Quaternion.identity);
        }
    }

    private void Update()
    {
        if (Camera.main.transform.position.x <= 11.0f)
        {
            _targetCamera = Camera.main.transform.position;
            _targetCamera.x = 18.0f;
        }
        else if(18.0f <= Camera.main.transform.position.x)
        {
            _targetCamera = Camera.main.transform.position;
            _targetCamera.x = 11.0f;
        }

        Camera.main.transform.position = Vector3.MoveTowards(Camera.main.transform.position, _targetCamera, 0.01f);


#if UNITY_EDITOR
        if(Input.GetMouseButtonUp(0))
        {
            SceneManager.LoadScene("InGame");
        }
#else
        if(Input.touchCount > 0)
        {
            SceneManager.LoadScene("InGame");
        }
#endif
    }

    void Blink()
    {
        bool active = StartText.activeSelf;
        StartText.gameObject.SetActive(!active);
    }

    void InitMapData()
    {
        for (int i = 0; i < _mapFloorHeight; i++)
            for (int j = 0; j < _mapFloorWidth; j++)
            {
                if (_tilemapHill.GetSprite(new Vector3Int(j, i, 0)) ||
                   _tilemapWall.GetSprite(new Vector3Int(j, i, 0)))
                    _mapData[i, j] = Define.MapTile.Wall;
            }

        GameObject[] go = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var item in go)
        {
            Vector3Int cell = GetCellIndex(item.transform.position);
            _mapData[cell.y, cell.x] = Define.MapTile.Obstacle;
        }

        go = GameObject.FindGameObjectsWithTag("BreakableObstacle");
        foreach (var item in go)
        {
            Vector3Int cell = GetCellIndex(item.transform.position);
            _mapData[cell.y, cell.x] = Define.MapTile.Breakable;
        }
    }

    public Vector3Int GetCellIndex(Vector3 posWorld)
    {
        return _tilemapFloor.WorldToCell(posWorld);
    }

    public Vector3 GetCellCenterWorld(Vector3Int cellindex)
    {
        return _tilemapFloor.GetCellCenterWorld(cellindex);
    }

    public Vector3 GetCellCenterWorld(Vector3 posWorld)
    {
        Vector3Int cell = _tilemapFloor.WorldToCell(posWorld);
        return GetCellCenterWorld(cell);
    }
}
