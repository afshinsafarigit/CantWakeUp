using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public float[] angles = { 0, 95 };
    public int openState;
    public GameObject parent;
    private float currentAngle;


    // Start is called before the first frame update
    void Start()
    {
        openState = 0;
    }

    void Update()
    {
        if (parent.transform.localEulerAngles.y != angles[openState])
        {
            currentAngle = Mathf.Lerp(parent.transform.localEulerAngles.y, angles[openState], 0.05f);

            parent.transform.localEulerAngles = new Vector3(0, currentAngle, 0);
        }
    }

    void OpenAndClose()
    {
        if (openState == 0)
        {
            openState = 1;
        }
        else
        {
            openState = 0;
        }
    }

    void Interact()
    {
        //Debug.Log("It's locked.");
        OpenAndClose();
    }
}
