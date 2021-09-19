//Not Used
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineSwitch : MonoBehaviour
{
    Outline outline;
    Camera cam;
    public float distanceAway = 3;
    void Start()
    {
        outline = GetComponent<Outline>();
        cam = FindObjectOfType<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 distance = cam.transform.position - transform.position;
        if(distance.magnitude > distanceAway)
        {
            if (outline.enabled == true)
            {
                outline.enabled = false;
            }
        }
        else
        {
            if(outline.enabled == false)
            {
                outline.enabled = true;
            }
        }
    }
}
