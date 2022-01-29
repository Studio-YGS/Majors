using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    GameObject streetSigns;
    
    public GameObject currentWordDisplay/*, wordPop*/;

    public GameObject[] wordDisplayObjects;
    
    TextMeshProUGUI[] streetChildren;

    bool whichStreetOut = true, animating; 

    void Start()
    {
        streetSigns = GameObject.Find("Streets_Signs");

        streetChildren = streetSigns.GetComponentsInChildren<TextMeshProUGUI>();
    }

    public void SwitchStreet(WordCollision wordCollision)
    {
        if (!animating)
        {
            animating = true;

            if (whichStreetOut)
            {
                streetChildren[1].text = wordCollision.street;

                if(streetChildren[0].text != streetChildren[1].text)
                {
                    whichStreetOut = false;
                }
            }
            else
            {
                streetChildren[0].text = wordCollision.street;

                if (streetChildren[0].text != streetChildren[1].text)
                {
                    whichStreetOut = true;
                }
            }

            if(streetChildren[0].text != streetChildren[1].text)
            {
                streetSigns.GetComponent<Animator>().SetBool("streetSwitch", whichStreetOut);
            }

            StartCoroutine(AnimationWait());

            if (currentWordDisplay != null)
            {
                currentWordDisplay.GetComponentInParent<Animator>().SetBool(currentWordDisplay.name, false);
            }

            for (int i = 0; i < wordDisplayObjects.Length; i++)
            {
                TextMeshProUGUI[] letters = wordDisplayObjects[i].GetComponentsInChildren<TextMeshProUGUI>();

                if (letters.Length == wordCollision.mainWord.ToIntArray().Length)
                {
                    currentWordDisplay = wordDisplayObjects[i];

                    currentWordDisplay.GetComponentInParent<Animator>().SetBool(currentWordDisplay.name, true);
                }
            }
        }
    }

    IEnumerator AnimationWait()
    {
        yield return new WaitForSeconds(1.2f);

        animating = false;

        StopCoroutine(AnimationWait());
    }

    public void HomeSwitch()
    {
        if (!animating)
        {
            animating = true;

            if (whichStreetOut)
            {
                streetChildren[1].text = "Home";

                if (streetChildren[0].text != streetChildren[1].text)
                {
                    whichStreetOut = false;
                }
            }
            else
            {
                streetChildren[0].text = "Home";

                if (streetChildren[0].text != streetChildren[1].text)
                {
                    whichStreetOut = true;
                }
            }

            if (streetChildren[0].text != streetChildren[1].text)
            {
                streetSigns.GetComponent<Animator>().SetBool("streetSwitch", whichStreetOut);
            }

            if (currentWordDisplay != null)
            {
                currentWordDisplay.GetComponentInParent<Animator>().SetBool(currentWordDisplay.name, false);
            }

            currentWordDisplay = wordDisplayObjects[1];

            currentWordDisplay.GetComponentInParent<Animator>().SetBool(currentWordDisplay.name, true);

            StartCoroutine(AnimationWait());
        }
    }
}
