using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{
    // Various variables and gameobjects //
    // Camera, duh
    public Camera head;
    // Player reticule
    public Image reticule;
    // Player collider
    public CapsuleCollider playerCollider;
    // Variable for interactable object
    public GameObject interactable;
    // Pause screen
    public GameObject pauseScreen;

    // Movement variables //
    // Interact distance
    public float maxDistance;
    // Standing movement speed
    public float standSpeed;
    // Crouching movement speed
    public float crouchSpeed;
    // Standing height
    public float standHeight;
    // Crouching height
    public float crouchHeight;
    // Standing up speed (0 - 1)
    public float standUpSpeed;
    // Crouching down speed (0 - 1)
    public float crouchDownSpeed;
    // Current movement speed
    private float speed;
    // Current height
    private float height;
    // Target height
    private float targetHeight;
    // Stand/Crouch transition speed
    private float standCrouchSpeed;

    // Camera stuff //
    // Mouse sensitivity
    public float sensitivity;
    // Lowest angle the camera will rotate to in vertical axis
    public float minY;
    // Highest angle the camera will rotate to in vertical axis
    public float maxY;
    // Variables to house rotation calculations
    private float rotationX;
    private float rotationY;
    private bool paused;

    private void Start()
    {
        // Set the starting speed as standing speed just in case.
        speed = standSpeed;
        // Set cursor invisible
        Cursor.visible = false;
        // Set cursor locked to the center of the game window
        Cursor.lockState = CursorLockMode.Locked;
        // Set mouseLocked boolean to true
        paused = false;
    }

    void Update()
    {
        // Character movement

        if(!paused) {
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

            // Crouch down
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                // Change movement speed to crouch speed
                speed = crouchSpeed;
                // Set the target camera height to crouch height
                targetHeight = crouchHeight;
                // Make the collider smaller
                playerCollider.height = crouchHeight + 0.05f;
                // Move the collider to floor level
                playerCollider.center = new Vector3(0, (crouchHeight + 0.05f) / 2, 0);
                // Set stand/crouch transition speed to crouchDown
                standCrouchSpeed = crouchDownSpeed;
            }
            // Stand up
            if (Input.GetKeyUp(KeyCode.LeftControl))
            {
                // Set movement speed to stand speed
                speed = standSpeed;
                // Set the target camera height to stand height
                targetHeight = standHeight;
                // Make the collider larger
                playerCollider.height = standHeight + 0.05f;
                // Move the collider to floor level
                playerCollider.center = new Vector3(0, (standHeight + 0.05f) / 2, 0);
                // Set stand/crouch transition speed to standUp
                standCrouchSpeed = standUpSpeed;
            }
            // Stand/crouch camera height transition
            // Check if the current camera height is different from the target height
            if (height != targetHeight)
            {
                // Calculate new height for each frame
                height = Mathf.Lerp(head.transform.localPosition.y, targetHeight, standCrouchSpeed);
                // Set the camera height each frame
                head.transform.localPosition = new Vector3(0, height, 0);
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
                    // Set reticule color to red
                    reticule.color = Color.red;
                }
                else
                {
                    // Set reticule color to black
                    reticule.color = Color.black;
                }
            }
            else
            {
                // Clear any previous hit objects
                interactable = null;
                // Set reticule color to black
                reticule.color = Color.black;
            }


            // Interaction keypresses //

            // Interact with interactable objects

            // Check if there is an object to interact with and if left mouse button is clicked
            if (interactable && Input.GetMouseButtonDown(0))
            {
                // Tell the interactable object to do something
                interactable.SendMessage("Interact");
            }
        }

        // Interaction keypresses continue //
        // Hide and lock the cursor

        // Check if the pause key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Check if game is paused
            if (paused)
            {
                // Set cursor visible
                Cursor.visible = false;
                // Set cursor free
                Cursor.lockState = CursorLockMode.Locked;
                // Set mouseLocked boolean to false
                paused = false;
                // Disable pause screen
                pauseScreen.SetActive(false);
            }
            else
            {
                // Set cursor invisible
                Cursor.visible = true;
                // Set cursor locked to the center of the game window
                Cursor.lockState = CursorLockMode.None;
                // Set mouseLocked boolean to true
                paused = true;
                // Enable pause screen
                pauseScreen.SetActive(true);
            }
        }

        // Reset the scene
        if(Input.GetKeyDown(KeyCode.R))
        {
            //Reload the current scene
            Scene scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.name);
        }
        
    }
}
