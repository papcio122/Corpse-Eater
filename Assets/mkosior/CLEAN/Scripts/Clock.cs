using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;

public class Clock : MonoBehaviour
{
    public TMP_Text textClock;
    private float time = 0.0f;
    public bool isRunning = false;

    private void displayTime()
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        textClock.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void resetTimer()
    {
        time = 0.0f;
        displayTime();
    }

    // Update is called once per frame
    void Update()
    {
        if(isRunning)
        {
            time += Time.deltaTime;
            displayTime();
        }
    }
}
