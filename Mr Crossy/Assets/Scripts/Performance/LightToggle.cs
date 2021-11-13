using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightToggle : MonoBehaviour
{
    GameObject[] lights;
    Transform player;
    public float maxDistance;
    public int activeCount;
    Dictionary<float, GameObject> distDic = new Dictionary<float, GameObject>();
    List<float> distances = new List<float>();
    void Start()
    {
        lights = GameObject.FindGameObjectsWithTag("Light");
        player = GameObject.Find("Fps Character").transform;
        //foreach (GameObject light in lights)
        //{
        //    light.GetComponentInChildren<Light>().enabled = false;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        //foreach(GameObject light in lights)
        //{
        //    float distance = Vector3.Distance(light.transform.position, player.position);
        //    RaycastHit hit;
        //    if (distance < maxDistance && activeCount < 8)
        //    {
        //        if (light.GetComponentInChildren<Light>().enabled == false)
        //        {
        //            light.GetComponentInChildren<Light>().enabled = true;
        //            activeCount += 1;
        //        }
        //        //if (Physics.Raycast(player.position, light.GetComponentInChildren<Collider>().transform.position - player.transform.position, out hit))
        //        //{
        //        //    if(hit.collider == light.GetComponentInChildren<Collider>())
        //        //    {
        //        //        if (light.GetComponentInChildren<Light>().enabled == false)
        //        //        {
        //        //            light.GetComponentInChildren<Light>().enabled = true;
        //        //            activeCount += 1;
        //        //        }
        //        //        //if (light.GetComponent<Renderer>() && light.GetComponent<Renderer>().isVisible || light.GetComponentInChildren<Renderer>().isVisible)
        //        //        //{

        //        //        //}
        //        //        //else
        //        //        //{
        //        //        //    if (light.GetComponentInChildren<Light>().enabled == true)
        //        //        //    {
        //        //        //        light.GetComponentInChildren<Light>().enabled = false;
        //        //        //    }
        //        //        //}
        //        //    }
        //        //    //else
        //        //    //{
        //        //    //    if (light.GetComponentInChildren<Light>().enabled == true)
        //        //    //    {
        //        //    //        light.GetComponentInChildren<Light>().enabled = false;
        //        //    //    }
        //        //    //}
        //        //}

        //    }
        //    //else
        //    //{
        //    //    if (light.GetComponentInChildren<Light>().enabled == true)
        //    //    {
        //    //        light.GetComponentInChildren<Light>().enabled = false;
        //    //    }
        //    //}

        //    if(activeCount >= 8 && light.GetComponentInChildren<Light>().enabled == true)
        //    {


        //        if (distance > maxDistance)
        //        {
        //            light.GetComponentInChildren<Light>().enabled = false;
        //            activeCount -= 1;
        //        }
        //        else if (Physics.Raycast(player.position, light.GetComponentInChildren<Collider>().transform.position - player.transform.position, out hit))
        //        {
        //            if (hit.collider != light.GetComponentInChildren<Collider>())
        //            {
        //                light.GetComponentInChildren<Light>().enabled = false;
        //                activeCount -= 1;
        //            }
        //        }
        //        //else
        //        //{
        //        //    Dictionary<float, GameObject> distDic = new Dictionary<float, GameObject>();
        //        //    foreach(GameObject obj in lights)
        //        //    {
        //        //        if(obj.GetComponentInChildren<Light>().enabled == true)
        //        //        {
        //        //            float dist = Vector3.Distance(obj.transform.position, player.position);

        //        //            distDic.Add(dist, obj);
        //        //        }
        //        //    }

        //        //    List<float> distances = new List<float>();
        //        //    distances = distDic.Keys.ToList();
        //        //    distances.Sort();
        //        //    GameObject furthestObj = distDic[distances[distances.Count - 1]];
        //        //    furthestObj.GetComponentInChildren<Light>().enabled = false;
        //        //    activeCount -= 1;
        //        //    distDic.Clear();
        //        //    distances.Clear();

        //        //}

        //    }
        //}

        foreach (GameObject light in lights)
        {
            float dist = Vector3.Distance(light.transform.position, player.position);
            distDic.Add(dist, light);

        }
        distances = distDic.Keys.ToList();
        distances.Sort();
        for (int i = 0; i < 7; i++)
        {
            distDic[distances[i]].GetComponentInChildren<Light>().renderMode = LightRenderMode.ForcePixel;
        }
        for (int i = 8; i < distances.Count; i++)
        {
            distDic[distances[i]].GetComponentInChildren<Light>().renderMode = LightRenderMode.ForceVertex;
        }
        distDic.Clear();
        distances.Clear();
    }
}
