using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightToggle : MonoBehaviour
{
    GameObject[] lights;
    Transform player;
    public float maxDistance;
    int activeCount;
    void Start()
    {
        lights = GameObject.FindGameObjectsWithTag("Light");
        player = GameObject.Find("Fps Character").transform;
        foreach (GameObject light in lights)
        {
            light.GetComponentInChildren<Light>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GameObject light in lights)
        {
            float distance = Vector3.Distance(light.transform.position, player.position);
            RaycastHit hit;
            if (distance < maxDistance && activeCount < 8)
            {
                if(Physics.Raycast(player.position, light.GetComponentInChildren<Collider>().transform.position - player.transform.position, out hit))
                {
                    if(hit.collider == light.GetComponentInChildren<Collider>())
                    {
                        if (light.GetComponentInChildren<Light>().enabled == false)
                        {
                            light.GetComponentInChildren<Light>().enabled = true;
                            activeCount += 1;
                        }
                        //if (light.GetComponent<Renderer>() && light.GetComponent<Renderer>().isVisible || light.GetComponentInChildren<Renderer>().isVisible)
                        //{

                        //}
                        //else
                        //{
                        //    if (light.GetComponentInChildren<Light>().enabled == true)
                        //    {
                        //        light.GetComponentInChildren<Light>().enabled = false;
                        //    }
                        //}
                    }
                    //else
                    //{
                    //    if (light.GetComponentInChildren<Light>().enabled == true)
                    //    {
                    //        light.GetComponentInChildren<Light>().enabled = false;
                    //    }
                    //}
                }

            }
            //else
            //{
            //    if (light.GetComponentInChildren<Light>().enabled == true)
            //    {
            //        light.GetComponentInChildren<Light>().enabled = false;
            //    }
            //}

            if(activeCount >= 8 && light.GetComponentInChildren<Light>().enabled == true)
            {
                
                if (Physics.Raycast(player.position, light.GetComponentInChildren<Collider>().transform.position - player.transform.position, out hit))
                {
                    if (hit.collider != light.GetComponentInChildren<Collider>())
                    {
                        light.GetComponentInChildren<Light>().enabled = false;
                        activeCount -= 1;
                    }
                }
                else if (distance > maxDistance)
                {
                    light.GetComponentInChildren<Light>().enabled = false;
                    activeCount -= 1;
                }
            }
        }

        
    }
}