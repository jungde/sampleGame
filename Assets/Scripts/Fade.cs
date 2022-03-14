using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    Image _image;
    string _sceneName;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    public void FadeIn()
    {
        CancelInvoke("StartFadeIn");
        CancelInvoke("StartFadeOut");

        InvokeRepeating("StartFadeIn", 0.0f, 0.05f);
    }

    public void FadeOut(string toSceneName)
    {
        CancelInvoke("StartFadeIn");
        CancelInvoke("StartFadeOut");

        gameObject.SetActive(true);
        _sceneName = toSceneName;

        InvokeRepeating("StartFadeOut", 0.0f, 0.05f);
    }

    void StartFadeIn()
    {
        Color color = _image.color;
        color.a -= 0.3f;

        if(color.a <= 0.0f)
        {
            color.a = 0.0f;
            _image.color = color;

            gameObject.SetActive(false);
            CancelInvoke("StartFade");
        }
        else
        {
            _image.color = color;
        }
    }

    void StartFadeOut()
    {
        Color color = _image.color;
        color.a += 0.3f;

        if (color.a >= 1.0f)
        {
            color.a = 1.0f;
            _image.color = color;

            gameObject.SetActive(false);
            CancelInvoke("StartFade");

            SceneManager.LoadScene(_sceneName);
        }
        else
        {
            _image.color = color;
        }
    }
}
