using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour
{
    Text scoreText;
    string startText;

    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<Text>();
        startText = scoreText.text;

        ScoreManager.ScoreReached += UpdateScore;
    }

    void UpdateScore(int score)
    {
        scoreText.text = startText + score;
    }
}
