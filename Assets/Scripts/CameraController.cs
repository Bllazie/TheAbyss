using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float sensivity = 2.0f;
    public float maxAngle = 80.0f;

    public float rotationX = 0.0f;
    
    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        transform.parent.Rotate(Vector3.up * mouseX * sensivity);

        rotationX -= mouseY * sensivity;
        rotationX = Mathf.Clamp(rotationX, -maxAngle, maxAngle);
        transform.localRotation = Quaternion.Euler(rotationX, 0, 0);
    }
}
