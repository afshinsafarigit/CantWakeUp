using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    // Camera, duh
    public Camera head;
    // Interact distance
    public float maxDistance;
    public GameObject interactable;
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
    private bool mouseLocked;

    private void Start()
    {
        mouseLocked = false;
    }

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

        // Camera raycast
        // Get interactable objects
        // Variable for the object hit by raycast
        RaycastHit hit;
        // Check if raycast hits anything
        if (Physics.Raycast(head.transform.position, head.transform.forward, out hit, maxDistance))
        {
            // Check if the hit object has the tag "Interactable"
            if (hit.collider.CompareTag("Interactable"))
            {
                // Store hit object
                interactable = hit.transform.gameObject;
            }
        }
        else
        {
            // Clear any previous hit objects
            interactable = null;
        }

        // Interaction keypresses //

        // Hide and lock the cursor

        // Check if the key F is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            // Check if mouse is locked already
            if (mouseLocked)
            {
                // Set cursor visible
                Cursor.visible = true;
                // Set cursor free
                Cursor.lockState = CursorLockMode.None;
                // Set mouseLocked boolean to false
                mouseLocked = false;
            }
            else
            {
                // Set cursor invisible
                Cursor.visible = false;
                // Set cursor locked to the center of the game window
                Cursor.lockState = CursorLockMode.Locked;
                // Set mouseLocked boolean to true
                mouseLocked = true;
            }
        }

        // Interact with interactable objects

        // Check if there is an object to interact with and if left mouse button is clicked
        if (interactable && Input.GetMouseButtonDown(0))
        {
            // Tell the interactable object to do something
            interactable.SendMessage("Interact");
        }
    }
}
