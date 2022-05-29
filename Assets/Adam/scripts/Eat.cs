using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Text.RegularExpressions;

public class Eat : MonoBehaviour
{
    public bool eatable = false;
    public Tilemap meat;
    public int points = 0;
    public GameObject general;
    public int requiredLevel = int.MaxValue;

    int addpts;
    string tileName;
    public int p_bonus_1 = 2;
    public int p_bonus_2 = 4;
    public int p_bonus_3 = 8;

    // Start is called before the first frame update
    void Start()
    {
        meat = GetComponent<Tilemap>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int wormLevel = collision.gameObject.GetComponentInParent<WormController>().level;
            Vector3 hitPosition = Vector3.zero;

            hitPosition.x = collision.gameObject.transform.position.x;
            hitPosition.y = collision.gameObject.transform.position.y;

            //Debug.Log(hitPosition);
            if (meat.WorldToCell(hitPosition) != null && (eatable || requiredLevel <= wormLevel))
            {
                if (meat.GetTile(meat.WorldToCell(hitPosition)) != null)
                {
                    tileName = meat.GetTile(meat.WorldToCell(hitPosition)).name;

                    ChoosePoints(tileName);
                    meat.SetTile(meat.WorldToCell(hitPosition), null);


                    general.GetComponent<Score>().updateScore(addpts);
                }
            }

        }
    }

    public void ChoosePoints(string name)
    {
        switch (name)
        {
            case "BONUS_1":
                addpts = p_bonus_1;
                break;
            case "BONUS_2":
                addpts = p_bonus_2;
                break;
            case "BONUS_3":
                addpts = p_bonus_3;
                break;
            default:
                addpts = 0;
                break;


        }
    }
}
