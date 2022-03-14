using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    GameObject _explosion;

    AudioSource _audioSourceBomb;
    AudioClip _audioClipBomb;

    void Start()
    {
        _explosion = Resources.Load<GameObject>("Prefabs/BombExplosion");

        _audioClipBomb = Resources.Load<AudioClip>("Jump14");
        _audioSourceBomb = gameObject.AddComponent<AudioSource>();
        _audioSourceBomb.pitch = 1.0f;
        _audioSourceBomb.clip = _audioClipBomb;
        _audioSourceBomb.Play();

        StartCoroutine("SetExplosion");
    }

    IEnumerator SetExplosion()
    {
        yield return new WaitForSeconds(2.0f);

        Instantiate(_explosion, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
