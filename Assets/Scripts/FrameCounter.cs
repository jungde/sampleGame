using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrameCounter : MonoBehaviour
{
    float deltaTime = 0f;
    public bool isShow;

    void Start()
    {
        isShow = true;
    }

    void Update()
    {
        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;

        if(Input.GetKeyDown(KeyCode.F1))
        {
            isShow = !isShow;
        }

        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Application.targetFrameRate = 30;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Application.targetFrameRate = 60;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Application.targetFrameRate = 144;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Application.targetFrameRate = -1;
        }
    }

    private void OnGUI()
    {
        if(isShow)
        {
            GUIStyle style = new GUIStyle();

            Rect rect = new Rect(30, 1390, Screen.width, Screen.height);
            style.alignment = TextAnchor.UpperLeft;
            style.fontSize = 50;
            style.normal.textColor = Color.green;

            float ms = deltaTime * 1000f;
            float fps = 1.0f / deltaTime;
            string text = string.Format("{0:0.} FPS ({1:0.0} ms) / (FrameRate : {2:D0}) / [{3:0.},{4:0.}] / [{5:}]", fps, ms, Application.targetFrameRate, Screen.width, Screen.height, Application.platform);

            GUI.Label(rect, text, style);
        }
    }
}
