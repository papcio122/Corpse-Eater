using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveController : MonoBehaviour
{
    public List<Transform> bodyParts = new List<Transform>();
    public GameObject bodyprefabs;

    public int beginSize;
    public float speed = 1;
    public float rotationSpeed = 1;

    Rigidbody2D headRigidBody;

    private void Awake()
    {
        headRigidBody = bodyParts[0].GetComponent<Rigidbody2D>();
    }

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
    }

    public void Move()
    {
        float curspeed = speed;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        if (vertical != 0 || horizontal != 0)
        {
            Vector3 target = new Vector3(-vertical * float.MaxValue, horizontal * float.MaxValue);
            Vector2 direction = target - bodyParts[0].transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            bodyParts[0].transform.rotation = Quaternion.Slerp(bodyParts[0].transform.rotation, rotation, rotationSpeed * Time.deltaTime);
        }

        headRigidBody.velocity = -bodyParts[0].transform.up * speed;
        //bodyParts[0].transform.Translate(-1 * bodyParts[0].transform.up * curspeed * Time.smoothDeltaTime, Space.World);
    }

    public void AddBodyPart()
    {
        Transform newpart = (Instantiate(bodyprefabs, bodyParts[bodyParts.Count - 1].position, bodyParts[bodyParts.Count - 1].rotation) as GameObject).transform;

        newpart.SetParent(transform);
        HingeJoint2D hingeJoint = newpart.GetComponent<HingeJoint2D>();
        hingeJoint.connectedBody = bodyParts[bodyParts.Count - 1].GetComponent<Rigidbody2D>();

        bodyParts.Add(newpart);
    }
}
