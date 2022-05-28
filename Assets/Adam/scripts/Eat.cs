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
    public int p_flesh = 1;
    public int p_bonus_1 = 2;
    public int p_bonus_2 = 3;
    public int p_bonus_3 = 4;
    public int p_bone_w = 10;
    public int p_bone_v = 20;
    public int p_bone_b = 30;
    public int p_bone_g = 40;

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
                tileName = meat.GetTile(meat.WorldToCell(hitPosition)).name;
                ChoosePoints(tileName);
                meat.SetTile(meat.WorldToCell(hitPosition), null);

                general.GetComponent<Score>().updateScore(addpts);
            }

        }
    }

    public void ChoosePoints(string name)
    {
        switch (name)
        {
            case "FLESH":
                addpts = p_flesh;
                break;
            case "BONUS_1":
                addpts = p_bonus_1;
                break;
            case "BONUS_2":
                addpts = p_bonus_2;
                break;
            case "BONUS_3":
                addpts = p_bonus_3;
                break;
            case var bone when new Regex(@"WBONE\w+").IsMatch(bone):
                addpts = p_bone_w;
                break;
            case var bone when new Regex(@"VBONE\w+").IsMatch(bone):
                addpts = p_bone_v;
                break;
            case var bone when new Regex(@"BBONE\w+").IsMatch(bone):
                addpts = p_bone_b;
                break;
            case var bone when new Regex(@"GBONE\w+").IsMatch(bone):
                addpts = p_bone_g;
                break;
            default:
                addpts = 0;
                break;


        }
    }
}
