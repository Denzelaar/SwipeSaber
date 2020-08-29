using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public ObjSpawner objSpawner;

    float intervalTime;
    float timer;

    public void Init(float interval, int poolSize)
    {
        objSpawner.Init(poolSize);
        intervalTime = interval;

        PhaseManager.NewPhase += ImplementPhase;

    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    void Timer()
    {
        timer += Time.deltaTime;

        if (timer > intervalTime)
        {
            timer = 0;
            objSpawner.SpawnObject();
        }
    }

    void ImplementPhase(PhaseDetails phase)
    {
        objSpawner.PhaseChange(phase);
    }
}
