using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class regionalCuller : MonoBehaviour
{
    public GameObject[] region;
    public int   totalRegions;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameController"))
        {
            VisibilityOn();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GameController"))
        {
            VisibilityOff();
        }
    }
    public void VisibilityOn()
    {
        foreach (GameObject houses in region)
        {
            houses.SetActive(true);
        }
    }
    public void VisibilityOff()
    {
        foreach (GameObject houses in region)
        {
            houses.SetActive(false);
        }
    }
}
