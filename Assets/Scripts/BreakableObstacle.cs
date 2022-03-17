using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableObstacle : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Invoke("DestroyObstacle", 0.7f);
        
    }

    void DestroyObstacle()
    {
        Destroy(gameObject);
    }
}
