using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;

public class Score : MonoBehaviour
{
    public TMP_Text textScore;
    public TMP_Text nextLife;
    public int score = 0;

    public int previousSegmentPts = 0;
    public int currentSegmentPts = 0;
    public int nextSegmentPts = 1;

    public WormController wormController;

    private void displayScore()
    {
        textScore.text = string.Format("{0:000000}", score);
        nextLife.text = nextSegmentPts.ToString();
    }

    public void Start()
    {
        wormController = GameObject.FindObjectOfType<WormController>();
    }

    public void updateScore(int val)
    {
        score += val;
        displayScore();
        while (score >= nextSegmentPts)
        {
            wormController.AddBodyPart();
            previousSegmentPts = currentSegmentPts;
            currentSegmentPts = nextSegmentPts;
            nextSegmentPts = currentSegmentPts + previousSegmentPts;
        }
    }
}
