using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class ExternalInteralSwitch : MonoBehaviour
{
    public GameObject[] outdoorThreeDSounds;
    public GameObject[] outdoorTwoDSounds;
    public GameObject[] indoorThreeDSounds;
    public GameObject[] indoorTwoDSounds;


    public int floors = 0;



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("GameController"))
        {
            floors++;
            WalkIn();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("GameController"))
        {
            if (floors >= 1)
            {
                WalkOut();
            }
        }
    }
    public void WalkIn()
    {
        foreach (GameObject threeDSound in outdoorThreeDSounds)
        {
            threeDSound.SetActive(false);
        }
        foreach (GameObject twoDSound in outdoorTwoDSounds)
        {
            twoDSound.GetComponent<StudioEventEmitter>().Stop();
        }
        foreach (GameObject indoorTwoD in indoorTwoDSounds)
        {
            indoorTwoD.GetComponent<StudioEventEmitter>().Play();
        }
        foreach (GameObject indoorThreeD in indoorThreeDSounds)
        {
            indoorThreeD.SetActive(true);
        }
    }
    public void WalkOut()
    {
        foreach (GameObject threeDSound in outdoorThreeDSounds)
        {
            threeDSound.SetActive(true);
        }
        foreach (GameObject twoDSound in outdoorTwoDSounds)
        {
            twoDSound.GetComponent<StudioEventEmitter>().Play();
        }
        foreach (GameObject indoorTwoD in indoorTwoDSounds)
        {
            indoorTwoD.GetComponent<StudioEventEmitter>().Stop();
        }
        foreach (GameObject indoorThreeD in indoorThreeDSounds)
        {
            indoorThreeD.SetActive(false);
        }
    }
}
