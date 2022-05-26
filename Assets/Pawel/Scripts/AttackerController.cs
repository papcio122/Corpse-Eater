using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerController : MonoBehaviour
{
    public float startSize;
    public float timeToHit;
    public Sprite attackSprite;
    public float timeToDestroy;

    public float timeSinceSpawn;
    public bool readyToHit;

    // Start is called before the first frame update
    void Start()
    {
        readyToHit = false;
        timeSinceSpawn = 0;
        GetComponent<Collider2D>().enabled = false;
        transform.localScale *= startSize;
    }

    // Update is called once per frame
    void Update()
    {
        if (timeSinceSpawn >= timeToHit)
        {
            if (timeToDestroy > 0)
            {
                timeToDestroy -= Time.deltaTime;
            } else
            {
                Destroy(gameObject);
            }
            if (!readyToHit)
            {
                GetComponent<Collider2D>().enabled = true;
                readyToHit = true;
                GetComponent<SpriteRenderer>().sprite = attackSprite;
            }
        } else
        {
            float T = timeSinceSpawn / timeToHit;
            transform.localScale = Vector3.Lerp(Vector3.one * startSize, Vector3.one, T);
        }
        timeSinceSpawn += Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        WormController worm = collision.gameObject.GetComponentInParent<WormController>();
        if (worm)
        {
            worm.RemoveBodyParts(1);
            GetComponent<Collider2D>().enabled = false;
        }
    }
}
