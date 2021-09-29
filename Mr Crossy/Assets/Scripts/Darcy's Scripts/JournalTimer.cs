using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class JournalTimer : MonoBehaviour
{
    public static JournalTimer instance;

    [SerializeField]
    TextMeshProUGUI timerText;

    TimeSpan timePlaying;
    bool timerGoing = false;

    float elapsedTime;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartTimer();
    }

    public void StartTimer() //starts from 0 
    {
        timerGoing = true;
        elapsedTime = 0f;

        StartCoroutine(UpdateTimer());
    }

    public void UnpauseTimer() //use this to unpause so the timer doesn't restart from 0
    {
        timerGoing = true;
    }

    public void EndTimer() //this can be used to pause or completely end the timer
    {
        timerGoing = false;
    }

    IEnumerator UpdateTimer()
    {
        while (timerGoing)
        {
            elapsedTime += Time.deltaTime;
            timePlaying = TimeSpan.FromSeconds(elapsedTime);

            string textStr = "Time: " + timePlaying.ToString("hh':'mm':'ss"); //formatted into hours, minutes and seconds
            timerText.text = textStr;

            yield return null;
        }
    }
}
