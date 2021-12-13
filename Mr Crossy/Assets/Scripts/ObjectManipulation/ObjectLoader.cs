using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectLoader : MonoBehaviour
{
    public GameObject[] houseInternals;
    void Start()
    {
        foreach (GameObject internals in houseInternals)
        {
            internals.SetActive(true);

        }
        foreach (GameObject internals in houseInternals)
        {
            
            internals.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
