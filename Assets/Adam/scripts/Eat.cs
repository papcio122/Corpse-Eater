using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Eat : MonoBehaviour
{
    public Tilemap meat;
    public int points = 0;
    public GameObject general;

    // Start is called before the first frame update
    void Start()
    {
        meat = GetComponent<Tilemap>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 hitPosition = Vector3.zero;

            hitPosition.x = collision.gameObject.transform.position.x;
            hitPosition.y = collision.gameObject.transform.position.y;

            Debug.Log(hitPosition);
            if (meat.WorldToCell(hitPosition) != null)
            {
                meat.SetTile(meat.WorldToCell(hitPosition), null);
                //points++;
                general.GetComponent<General>().score++;

            }

        }
    }
}
