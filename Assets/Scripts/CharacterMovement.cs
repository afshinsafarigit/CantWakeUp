using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Camera, duh
    public Camera head;
    // Character movement speed
    public int speed;
    // Mouse sensitivity
    public float sensitivity;
    // Lowest angle the camera will rotate to in vertical axis
    public float minY;
    // Highest angle the camera will rotate to in vertical axis
    public float maxY;
    // Variables to house rotation calculations
    private float rotationX;
    private float rotationY;

    void Update()
    {
        // Character movement
        // Moves character forward or backward
        if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(new Vector3(0, 0, Input.GetAxis("Vertical") * speed * Time.deltaTime));
        }
        // Moves character sideways
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(new Vector3(Input.GetAxis("Horizontal") * speed * Time.deltaTime, 0, 0));
        }

        // Viewpoint rotation

        // Calculate horizontal rotation
        rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivity;

        //Calculate vertical rotation
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, minY, maxY);

        // Rotate character for the horizontal rotation (camera is following the character)
        transform.localEulerAngles = new Vector3(0, rotationX, 0);
        // Rotate only the camera in vertical rotation (so that the character model doesn't tilt)
        head.transform.localEulerAngles = (new Vector3(-rotationY, head.transform.localEulerAngles.y, 0));

    }
}
