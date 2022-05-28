using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform follow;
    public float followSpeed = 0.5f;
    public Camera cameraComponent;
    public float targetSize = 5f;
    public float sizeSpeed = 20f;
    public float newSize;

    // Start is called before the first frame update
    void Start()
    {
        follow = GameObject.FindGameObjectWithTag("Head").transform;
        //transform.position = follow.position;
        cameraComponent = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        float dis = Vector3.Distance(follow.position, transform.position);

        float t = Time.deltaTime * dis * followSpeed;

        if (cameraComponent.orthographicSize > targetSize)
        {
            newSize = Mathf.MoveTowards(cameraComponent.orthographicSize, targetSize, Time.deltaTime * sizeSpeed);
            cameraComponent.orthographicSize = newSize;
            t /= sizeSpeed;
        }

        Vector3 newPos = Vector3.Lerp(transform.position, follow.position, t);
        newPos.z = -10;
        transform.position = newPos;
    }
}
