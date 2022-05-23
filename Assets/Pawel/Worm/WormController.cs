using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour
{

    public List<Transform> bodyParts = new List<Transform>();

    public float minDistance = 0.25f;

    public int beginSize;

    public float speed = 1;
    public float rotationSpeed = 50;

    public GameObject bodyprefabs;

    private float dis;
    private Transform curBodyPart;
    private Transform PrevBodyPart;


    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < beginSize - 1; i++)
        {
            AddBodyPart();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Move();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddBodyPart();
        }
    }

    public void Move()
    {
        float curspeed = speed;

        if (Input.GetAxis("Horizontal") != 0)
        {
            bodyParts[0].Rotate(Vector3.forward * rotationSpeed * Time.deltaTime * Input.GetAxis("Horizontal"));
        }

        bodyParts[0].Translate(-1*bodyParts[0].up * curspeed * Time.smoothDeltaTime, Space.World);

        for (int i = 1; i < bodyParts.Count; i++)
        {
            curBodyPart = bodyParts[i];
            PrevBodyPart = bodyParts[i - 1];

            dis = Vector3.Distance(PrevBodyPart.position, curBodyPart.position);

            Vector3 newpos = PrevBodyPart.position;

            newpos.y = bodyParts[0].position.y;
            newpos.x = bodyParts[0].position.x;

            float T = Time.deltaTime * dis / minDistance * curspeed;
            curBodyPart.position = Vector3.Slerp(curBodyPart.position, newpos, T);
            curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, PrevBodyPart.rotation, T);

        }
    }


    public void AddBodyPart()
    {
        Transform newpart = (Instantiate(bodyprefabs, bodyParts[bodyParts.Count - 1].position, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;

        newpart.SetParent(transform);

        bodyParts.Add(newpart);
    }
}