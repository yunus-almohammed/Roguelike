using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;

    public float minX = -40f;
    public float maxX = 41f;
    public float minY = -40f;
    public float maxY = 41f;

    void Update()
    {
        float camHeight = Camera.main.orthographicSize;
        float camWidth = camHeight * Camera.main.aspect;

        float x = Mathf.Clamp(
            target.position.x + offset.x,
            minX + camWidth,
            maxX - camWidth
        );

        float y = Mathf.Clamp(
            target.position.y + offset.y,
            minY + camHeight,
            maxY - camHeight
        );

        transform.position = new Vector3(x, y, offset.z);
    }
}