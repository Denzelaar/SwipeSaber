using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public delegate void OnScoreReached(int score);
    public static OnScoreReached ScoreReached;

    public int Score { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckScore()
    {
        if(Score > 1)
        {
            ScoreReached.Invoke(Score);
        }
    }
}
