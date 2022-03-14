using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void ExplosionComplete()
    {
        if (transform.parent.gameObject)
        {
            Destroy(transform.parent.gameObject);            
        }
    }
}
