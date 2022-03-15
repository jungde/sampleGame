using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Input_ByPC : MonoBehaviour
{
    public PlayerController _player = default;

    void Start()
    {
        
    }

    void Update()
    {
        if (Application.platform != RuntimePlatform.WindowsEditor) return;
        if (_player.State == Define.State.Die) 
            return;

            if (Input.GetKeyUp(KeyCode.A) ||
            Input.GetKeyUp(KeyCode.D) ||
            Input.GetKeyUp(KeyCode.W) ||
            Input.GetKeyUp(KeyCode.S) ||
            Input.GetKeyUp(KeyCode.LeftArrow)  ||
            Input.GetKeyUp(KeyCode.RightArrow) ||
            Input.GetKeyUp(KeyCode.UpArrow)    ||
            Input.GetKeyUp(KeyCode.DownArrow))
        {
            _player.NoMove();
            return;
        }

        if (Input.GetAxisRaw("Horizontal") < 0.0f)
            _player.LeftMove();
        else if (Input.GetAxisRaw("Horizontal") > 0.0f)
            _player.RightMove();

        if (Input.GetAxisRaw("Vertical") < 0.0f)
            _player.DownMove();
        else if (Input.GetAxisRaw("Vertical") > 0.0f)
            _player.UpMove();

        if (Input.GetKeyDown(KeyCode.Space))
            _player.SetupBomb();
    }
}
