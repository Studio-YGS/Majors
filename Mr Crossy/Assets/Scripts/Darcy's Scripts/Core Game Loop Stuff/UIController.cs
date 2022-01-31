using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public GameObject streetSigns/*, wordPop*/;

    public GameObject[] wordDisplayObjects;

    bool whichStreetOut = true, animating; //true = a, false = b;

    void Start()
    {
        streetSigns = GameObject.Find("Streets_Signs");
    }

    public void SwitchStreet(WordCollision wordCollision)
    {
        if (!animating)
        {
            if (whichStreetOut)
            {
                whichStreetOut = false;
            }
            else
            {
                whichStreetOut = true;
            }

            Debug.Log("Which Street Out: " + whichStreetOut);

            animating = true; 
            streetSigns.GetComponent<Animator>().SetBool("streetSwitch", whichStreetOut);

            StartCoroutine(AnimationWait());
        }
    }

    IEnumerator AnimationWait()
    {
        yield return new WaitForSeconds(1.2f);

        animating = false;
    }
}
