using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;


[RequireComponent(typeof(StudioEventEmitter))]
public class AudioRandomiser : MonoBehaviour
{

    [Tooltip("Minimum time between sounds")]
    public float minWait = 5f;
    [Tooltip("Maximum time between sounds")]
    public float maxWait = 30f;
    //[Tooltip("Drag the Gameobject which has the Studio Event Emitter with the multisound you wish to randomise")]
    //public StudioEventEmitter multiSound;



    private void Start()
    {
        StartCoroutine("Waiter");
    }

    public IEnumerator Waiter()
    {

        float wait_time = Random.Range(minWait, maxWait);
        yield return new WaitForSeconds(wait_time);
        //Debug.Log("Got ready to play sound");
        StartCoroutine("RandomiseSounds");
    }
    public IEnumerator RandomiseSounds()
    {
        gameObject.GetComponent<StudioEventEmitter>().Play();
        yield return new WaitForSeconds(0F);
        StartCoroutine("Waiter");
    }
}


