using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using TMPro;

public class Score : MonoBehaviour
{
    public TMP_Text textScore;
    public int score = 0;

    private void displayScore()
    {
        textScore.text = string.Format("{0:000000}", score);
    }

    public void updateScore(int val)
    {
        score += val;
        displayScore();
    }
}
