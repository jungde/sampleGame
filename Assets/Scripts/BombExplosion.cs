using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    Scene_InGame _gameScene;

    void Start()
    {
        _gameScene = GameObject.Find("@Scene").GetComponent<Scene_InGame>();
        Invoke("ExplosionComplete", 0.7f);
    }

    void Update()
    {
        
    }

    public void ExplosionComplete()
    {
        if (transform.gameObject)
        {
            Destroy(transform.gameObject);

            if (_gameScene._showPopup == false)
            {
                GameObject[] go = GameObject.FindGameObjectsWithTag("Enemy");
                if (go.Length == 0)
                {
                    _gameScene._gapTime = 0.0f;
                    Instantiate(_gameScene._popup_Success, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
                    _gameScene._showPopup = true;
                }
            }
        }
    }
}
