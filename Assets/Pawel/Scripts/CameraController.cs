using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform follow;

    // Start is called before the first frame update
    void Start()
    {
        follow = GameObject.FindGameObjectWithTag("Head").transform;
    }

    // Update is called once per frame
    void Update()
    {
        float dis = Vector3.Distance(follow.position, transform.position);

        float T = Time.deltaTime * dis * 0.5f;
        Vector3 newPos = Vector3.Lerp(transform.position, follow.position, T);
        newPos.z = -10;
        transform.position = newPos;
    }
}
