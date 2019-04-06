using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserEmitter : MonoBehaviour
{
    public bool enabledFromStart;
    public GameObject laserPrefab;
    private GameObject laser;
    private Transform laserTransform;
    private Laser laserLaser;
    private bool turnedOn;
    private GameObject target;

    // Start is called before the first frame update
    void Start()
    {
        laser = Instantiate(laserPrefab);
        laserTransform = laser.transform;
        laserLaser = laser.GetComponent<Laser>();
        turnedOn = false;
        laser.SetActive(false);
        gameObject.GetComponent<Renderer>().enabled = enabledFromStart;
    }

    void Update()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.forward, out hit, 100))
        {
            laserTransform.position = Vector3.Lerp(transform.position, hit.point, .5f);
            laserTransform.LookAt(hit.point);
            laserTransform.localScale = new Vector3(laserTransform.localScale.x, laserTransform.localScale.y, hit.distance);
            laserLaser.hitPoint = hit.point;

            if (turnedOn)
            {
                if (hit.transform.name.Contains("LaserEmitter"))
                {
                    if (!target)
                    {
                        hit.transform.tag = "Interactable";
                        hit.transform.GetComponent<Renderer>().enabled = true;
                        target = hit.transform.gameObject;
                    }
                }
                else if (hit.transform.name.Contains("Body"))
                {
                    Debug.Log("You win");
                }
                else if (target)
                {
                    hit.transform.tag = "";
                    target.GetComponent<Renderer>().enabled = false;
                    target = null;
                }
            }
            else if (target)
            {
                target.GetComponent<Renderer>().enabled = false;
                target = null;
            }
        }

        if (!gameObject.GetComponent<Renderer>().enabled && turnedOn)
        {
            Interact();
        }
    }

    void Interact()
    {
        turnedOn = !turnedOn;

        laser.SetActive(turnedOn);
    }
}
