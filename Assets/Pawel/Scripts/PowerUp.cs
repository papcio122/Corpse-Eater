using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Head"))
        {
            WormController worm = collision.gameObject.GetComponentInParent<WormController>();
            if (worm)
            {
                worm.LevelUp();
                Debug.Log("LevelUP");
                Destroy(gameObject);
            }
        }
    }
}
