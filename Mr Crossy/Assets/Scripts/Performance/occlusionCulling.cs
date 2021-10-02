using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Script written by Sketch
public class occlusionCulling : MonoBehaviour
{
    void Start()
    {
        Camera camera = GetComponent<Camera>();//Gets player camera
        float[] distances = new float[32]; //float arrach representing all layers
        distances[10] = 15; //Set cull distance of layer 10 to 15
        camera.layerCullDistances = distances; //applies changes
    }
}
