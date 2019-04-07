using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorController : MonoBehaviour
{
    // Possible angles for mirror
    private readonly float[] angles = {180f, 225f, 135f};
    // Current angle state variable
    private int angleState;

    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Vector3 reflectionPoint;

    // Start is called before the first frame update
    void Start()
    {
        reflectionPoint = transform.position;
        angleState = 0;
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        laser.SetActive(false);
    }

    void FixedUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(reflectionPoint, transform.forward, out hit, 100))
        {
            laserTransform.position = Vector3.Lerp(reflectionPoint, hit.point, .5f);
            laserTransform.LookAt(hit.point);
            laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
            laser.GetComponent<Laser>().hitPoint = hit.point;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Ree");
        if (other.CompareTag("Laser"))
        {
            reflectionPoint = other.GetComponent<Laser>().hitPoint;
            laser.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Laser"))
        {
            laser.SetActive(false);
        }
    }

    void Interact()
    {
        angleState++;
        if (angleState > 2)
        {
            angleState = 0;
        }

        transform.localEulerAngles = new Vector3(0, angles[angleState], 0);

        RaycastHit hit;

        if (Physics.Raycast(reflectionPoint, transform.forward, out hit, 100))
        {
            laserTransform.position = Vector3.Lerp(reflectionPoint, hit.point, .5f);
            laserTransform.LookAt(hit.point);
            laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
        }
    }
}
