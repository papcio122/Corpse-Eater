using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform follow;
    public float followSpeed = 0.5f;
    public Camera cameraComponent;
    public float defaultSize = 5f;
    public float scaleSpeed = 20f;
    public float newSize;
    public float zoomScale = 5;
    public Vector3 finishPosition;
    public float finishSize;
    public bool finished = false;

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
        float targetSize = defaultSize;
        Vector3 targetPosition = follow.position;
        float dis = Vector3.Distance(targetPosition, transform.position);

        float t = Time.deltaTime * dis * followSpeed;
        if (Input.GetButton("Fire1"))
        {
            targetSize *= zoomScale;
        }

        if (finished)
        {
            targetSize = finishSize;
            targetPosition = finishPosition;
        }

        if (cameraComponent.orthographicSize != targetSize)
        {
            newSize = Mathf.MoveTowards(cameraComponent.orthographicSize, targetSize, Time.deltaTime * scaleSpeed);
            cameraComponent.orthographicSize = newSize;
            t /= scaleSpeed;
        }

        Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, t);
        newPos.z = -10;
        transform.position = newPos;
    }
}
