using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreManager : Singleton<ScoreManager>
{
    public delegate void OnScoreReached(int score);
    public static OnScoreReached ScoreReached;

    public int Score { get; private set; }

    // Start is called before the first frame update
    public void ScoreInit()
    {
        Block.BlockHit += BlockHit;
    }

    void BlockHit()
    {
        Score++;
        ScoreReached.Invoke(Score);
    }

    private void OnDisable()
    {
        Block.BlockHit -= BlockHit;

    }
}
