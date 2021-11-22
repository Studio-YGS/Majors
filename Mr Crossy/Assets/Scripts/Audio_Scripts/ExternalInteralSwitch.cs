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



    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("GameController"))
    //    {
    //        floors++;
    //        WalkIn();
    //        FindObjectOfType<OverseerController>().m_PlayerInHouse = true;
    //    }
    //}
    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.CompareTag("GameController"))
    //    {
    //        if (floors >= 2)
    //        {
    //            WalkOut();
    //            FindObjectOfType<OverseerController>().m_PlayerInHouse = false;
    //        }
    //    }
    //}
    public void WalkIn()
    {
        foreach (GameObject threeDSound in outdoorThreeDSounds)
        {
            threeDSound.SetActive(false);
        }
        foreach (GameObject twoDSound in outdoorTwoDSounds)
        {
            twoDSound.SetActive(false);
        }
        foreach (GameObject indoorTwoD in indoorTwoDSounds)
        {
            indoorTwoD.SetActive(true);
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
            twoDSound.SetActive(true);
        }
        foreach (GameObject indoorTwoD in indoorTwoDSounds)
        {
            indoorTwoD.SetActive(false);
        }
        foreach (GameObject indoorThreeD in indoorThreeDSounds)
        {
            indoorThreeD.SetActive(false);
        }
    }
}
