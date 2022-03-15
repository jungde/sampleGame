using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Scene_InGame_Popup : MonoBehaviour
{
    Button btnContinue;
    Button btnExit;

    void Start()
    {
        btnContinue = GameObject.Find("Continue").GetComponent<Button>();
        btnContinue.onClick.AddListener(delegate { OnClickContinue(); });
        btnExit = GameObject.Find("Exit").GetComponent<Button>();
        btnExit.onClick.AddListener(delegate { OnClickExit(); });
    }

    void OnClickContinue()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnClickExit()
    {
        Application.Quit();
    }
}
