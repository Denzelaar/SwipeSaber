using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneralManager : Singleton<GeneralManager>
{
    public delegate void OnRequestNewPhase(int score);
    public static OnRequestNewPhase RequestNewPhase;

    public ScoreManager scoreManager;
    public SpawnManager spawnManager;
    public PhaseManager phaseManager;

    public float timerInterval;
    public int poolSize;

    // Start is called before the first frame update
    void Start()
    {
        Phases.Instance.StartFill();

        phaseManager.PhaseInit();
        spawnManager.Init(timerInterval, poolSize);
        scoreManager.ScoreInit();

        ScoreManager.ScoreReached += NewPhaseRequest;

        NewPhaseRequest(10);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void NewPhaseRequest(int score)
    {
        RequestNewPhase.Invoke(score);

    }
}
