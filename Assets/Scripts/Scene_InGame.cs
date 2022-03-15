using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class Scene_InGame : MonoBehaviour
{
    Grid _grid;
    Tilemap _tilemapHill;
    Tilemap _tilemapWall;
    public Tilemap _tilemapFloor;
    public GameObject _startPoint;
    TextMeshProUGUI _RemainingTime;

    int _mapFloorWidth = 29;
    int _mapFloorHeight = 13;
    public MapTile[,] _mapData = default;

    float time;

    public enum MapTile
    {
        Floor,
        Wall,
        Obstacle,
        Breakable,
    }

    private void Awake()
    {
        if(GameObject.FindGameObjectWithTag("Fade"))
            GameObject.FindGameObjectWithTag("Fade").GetComponent<Fade>().FadeIn();

        _grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grid>();
        _tilemapHill = GameObject.Find("Tilemap_Hill").GetComponent<Tilemap>();
        _tilemapWall = GameObject.Find("Tilemap_Wall").GetComponent<Tilemap>();
        _tilemapFloor = GameObject.Find("Tilemap_Floor").GetComponent<Tilemap>();
        _RemainingTime = GameObject.FindGameObjectWithTag("RemainingTime").GetComponent<TextMeshProUGUI>();

        _mapData = new MapTile[_mapFloorHeight, _mapFloorWidth];
        InitMapData();
        

        GameObject enemy = Resources.Load<GameObject>("Prefabs/blue1");
        GameObject[] gos = GameObject.FindGameObjectsWithTag("SpawnPoint");
        List<GameObject> lgos = new List<GameObject>(gos);
        foreach(GameObject o in lgos)
        {
            Vector3Int cellIndex = _grid.WorldToCell(o.transform.position);
            Vector3 cellCenterWorld = _grid.GetCellCenterLocal(cellIndex);

            Instantiate(enemy, cellCenterWorld, Quaternion.identity);
        }

        GameObject player = Resources.Load<GameObject>("Prefabs/Player");
        _startPoint = GameObject.Find("@StartPosition");
        Vector3Int posStart = _tilemapFloor.WorldToCell(_startPoint.transform.position);
        Vector3 posCellCenter = _tilemapFloor.GetCellCenterLocal(posStart) - new Vector3(0.0f, 0.5f, 0.0f);
        GameObject instance = Instantiate(player, posCellCenter, Quaternion.identity);
    }

    void Start()
    {

    }

    void Update()
    {
        //float gap = 5.0f - Time.time;
        //if (gap <= 0)
        //    gap = 0;
        //_RemainingTime.text = string.Format("남은시간 {0:0.0}", gap);

        
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

    void InitMapData()
    {
        for (int i = 0; i < _mapFloorHeight; i++)
            for (int j = 0; j < _mapFloorWidth; j++)
            {
                if (_tilemapHill.GetSprite(new Vector3Int(j, i, 0)) ||
                   _tilemapWall.GetSprite(new Vector3Int(j, i, 0)))
                    _mapData[i, j] = MapTile.Wall;
            }

        GameObject[] go = GameObject.FindGameObjectsWithTag("Obstacle");
        foreach (var item in go)
        {
            Vector3Int cell = GetCellIndex(item.transform.position);
            _mapData[cell.y, cell.x] = MapTile.Obstacle;
        }

        go = GameObject.FindGameObjectsWithTag("BreakableObstacle");
        foreach (var item in go)
        {
            Vector3Int cell = GetCellIndex(item.transform.position);
            _mapData[cell.y, cell.x] = MapTile.Breakable;
        }
    }

    private void OnGUI()
    {
        //for (int i = 0; i < _mapFloorHeight; i++)
        //    for (int j = 0; j < _mapFloorWidth; j++)
        //    {
        //        GUIStyle style = new GUIStyle();
        //        Rect rect = new Rect(j * 30, i * 50 + 100, Screen.width, Screen.height);
        //        style.alignment = TextAnchor.UpperLeft;
        //        style.fontSize = 50;
        //        style.normal.textColor = Color.red;
        //        string text = string.Format("{0}", (int)_mapData[i, j]);
        //        GUI.Label(rect, text, style);
        //    }


    }
}
