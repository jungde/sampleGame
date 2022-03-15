using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Input_ByMobile : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public PlayerController _player = default;

    GraphicRaycaster _raycaster;
    Action action;

    Vector2 _InputDir
    {
        get; set;
    }

    void Start()
    {
        _raycaster = transform.parent.GetComponent<GraphicRaycaster>();

        if (_player == null) Debug.Log("NoSetting : Input_ByMobile._player");
    }

    void Update()
    {
        if(_player.State == Define.State.Die)
        {
            action -= _player.LeftMove;
            action -= _player.RightMove;
            action -= _player.UpMove;
            action -= _player.DownMove;
            _player.NoMove();
        }

        if(action != null)
            action.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(eventData, results);

        string name = results[0].gameObject.name;
        //Debug.Log($"{name}");

        if (name == "LeftButton")
            action = _player.LeftMove;
        if (name == "RightButton")
            action = _player.RightMove;
        if (name == "UpButton")
            action = _player.UpMove;
        if (name == "DownButton")
            action = _player.DownMove;

        if (name == "A_Key")
            _player.SetupBomb();

        //Debug.Log("OnPointerDown");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        action -= _player.LeftMove;
        action -= _player.RightMove;
        action -= _player.UpMove;
        action -= _player.DownMove;
        _player.NoMove();

        //Debug.Log("OnPointerUp");
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }


}
