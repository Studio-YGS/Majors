using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.VFX;

public class UIController : MonoBehaviour //this script controls the UI animations for the street names and the word boxes that the letters appear in.
{
    GameObject streetSigns;

    public GameObject wordPop;

    [HideInInspector]
    public GameObject currentWordDisplay;

    public GameObject[] wordDisplayObjects;
    
    TextMeshProUGUI[] streetChildren;

    bool whichStreetOut = true, animating; 

    void Start()
    {
        streetSigns = GameObject.Find("Streets_Signs");

        streetChildren = streetSigns.GetComponentsInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {                       //testing
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Attempting to play");
            wordPop.GetComponent<VisualEffect>().Play();
        }
    }

    public void SwitchStreet(WordCollision wordCollision) //this method switches out the two street signs and changes the text to reflect the street that the player is on
    {
        if (!animating) //locks it from happening more than once every 1.2 seconds
        {
            animating = true;

            if (whichStreetOut) //if which street out is true, it means street sign A is out, and vice versa
            {
                streetChildren[1].text = wordCollision.street;

                if(streetChildren[0].text != streetChildren[1].text) //if the text is the same as the other sign, it won't switch the bool around, to avoid it becoming opposite.
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

            if(streetChildren[0].text != streetChildren[1].text) //if the text is the same as the other sign, it won't animate, as there is no point
            {
                streetSigns.GetComponent<Animator>().SetBool("streetSwitch", whichStreetOut);
            }

            StartCoroutine(AnimationWait()); //waiting for 1.2 seconds to change the animating bool back to false

            if (currentWordDisplay != null) //current word display will be null the first time the script comes through here, so a check to prevent errors 
            {
                currentWordDisplay.GetComponentInParent<Animator>().SetBool(currentWordDisplay.name, false); //pulls back the current word boxes that are out in the UI
            }

            for (int i = 0; i < wordDisplayObjects.Length; i++) //this loops finds which word box to pull out next
            {
                TextMeshProUGUI[] letters = wordDisplayObjects[i].GetComponentsInChildren<TextMeshProUGUI>(); //makes an array out of the children of each of the word objects

                if (letters.Length == wordCollision.mainWord.ToIntArray().Length) //if the length of the letters array is equal to the length of the word, it animates that word box object
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

    public void HomeSwitch() //this does the same as the above method, however is hard-coded due to the home not having a word collider script.
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
