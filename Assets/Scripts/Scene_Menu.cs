using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_Menu : MonoBehaviour
{
    [SerializeField]
    GameObject StartText = default;

    Vector3 _targetCamera;

    private void Awake()
    {
        InvokeRepeating("Blink", 0f, 0.5f);
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
}
