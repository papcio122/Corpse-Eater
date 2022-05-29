using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackerSpawnerController : MonoBehaviour
{
    public GameObject target;
    public GameObject attacker;
    public float interval;
    public float basicProbability;
    public float probabilityIncrease;
    public float maxDistance;
    public float firstAttack = 3f;

    public float currentProbability;
    public float timeSinceAttack;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Head");
        currentProbability = basicProbability;
        timeSinceAttack = 0;
    }

    // Update is called once per frame
    void Update()
    {
        firstAttack -= Time.deltaTime;
        if(firstAttack <= 0)
        {
            if (timeSinceAttack > interval)
            {
                if (Random.Range(1, 100) <= currentProbability)
                {
                    Vector2 attackPosition = target.transform.position + (Vector3)(Random.insideUnitCircle * maxDistance);
                    Instantiate(attacker, attackPosition, Quaternion.identity);
                    currentProbability = basicProbability;
                }
                else
                {
                    currentProbability += probabilityIncrease;
                }
                timeSinceAttack = 0;
            }
            else
            {
                timeSinceAttack += Time.deltaTime;
            }
        }
        
    }
}
