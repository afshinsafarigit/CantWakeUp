using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{

    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private bool turnedOn;

    // Start is called before the first frame update
    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        turnedOn = false;
        laser.SetActive(false);
    }

    void LateUpdate()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
            laserTransform.position = Vector3.Lerp(transform.position, hit.point, .5f);
            laserTransform.LookAt(hit.point);
            laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
            laser.GetComponent<Laser>().hitPoint = hit.point;
        }
    }

    void Interact()
    {
        turnedOn = !turnedOn;

        laser.SetActive(turnedOn);
    }
}
