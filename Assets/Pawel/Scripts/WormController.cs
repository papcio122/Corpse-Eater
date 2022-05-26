using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormController : MonoBehaviour
{

    public List<Transform> bodyParts = new List<Transform>();
    public GameObject bodyprefabs;
    public int beginSize;

    public float speed = 1;
    public float rotationSpeed = 50;
    public float backDistance = 2;

    public bool isSlowed = false;
    public float slowTime = 2f;


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
        if (Input.GetKeyDown(KeyCode.Q))
        {
            AddBodyPart();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RemoveBodyParts(3);
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            BounceBack();
        }

        Move();

        if (isSlowed)
        {
            slowTime -= Time.deltaTime;
            if (slowTime < 0)
            {
                isSlowed = false;
            }
        }
    }

    public void Move()
    {
        float curspeed = speed;
        if (isSlowed)
        {
            curspeed /= 2;
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (vertical != 0 || horizontal != 0)
        {
            Vector3 target = new Vector3(vertical * float.MaxValue, -horizontal * float.MaxValue);
            Vector2 direction = target - bodyParts[0].transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            bodyParts[0].transform.rotation = Quaternion.Slerp(bodyParts[0].transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        bodyParts[0].Translate(bodyParts[0].up * curspeed * Time.deltaTime, Space.World);

        for (int i = 1; i < bodyParts.Count; i++)
        {
            Transform curBodyPart = bodyParts[i];
            Transform PrevBodyPart = bodyParts[i - 1];

            float dis = Vector3.Distance(PrevBodyPart.position, curBodyPart.position);

            Vector3 newpos = PrevBodyPart.position;

            float T = Time.deltaTime * dis * curspeed;
            curBodyPart.position = Vector3.Lerp(curBodyPart.position, newpos, T);
            curBodyPart.rotation = Quaternion.Lerp(curBodyPart.rotation, PrevBodyPart.rotation, T);

        }
    }


    public void AddBodyPart()
    {
        Transform newpart = (Instantiate(bodyprefabs, bodyParts[bodyParts.Count - 1].position, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;

        newpart.SetParent(transform);

        bodyParts.Add(newpart);
    }

    public void RemoveBodyParts(int count)
    {
        if (count >= bodyParts.Count)
        {
            count = bodyParts.Count - 1;
        }
        for (int i = 0; i < count; i++)
        {
            int index = bodyParts.Count - 1;
            Destroy(bodyParts[index].gameObject);
            bodyParts.RemoveAt(index);
        }
    }

    public void BounceBack()
    {
        for (int i = bodyParts.Count - 1; i >= 0; i--)
        {
            bodyParts[i].Translate(bodyParts[i].up * backDistance, Space.World);
        }
        Slow(2);
    }

    public void Slow(float seconds)
    {
        if (!isSlowed)
        {
            isSlowed = true;
        }
        slowTime = seconds;
    }
}