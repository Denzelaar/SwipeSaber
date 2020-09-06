using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PhaseDetails
{
    public string PhaseName { get; private set; }
    public float SpawnSpeed { get; private set; }
    public float BlockSpeed { get; private set; }
    public float DoubleSidedSpawnChance { get; private set; }
    public float ObstacleChance { get; private set; }
    public Direction[] AllowedDirectoins { get; private set; }

    public PhaseDetails(string Name, float speed, float bs, float chanceOfDoubleSpawn,
    float obst, Direction[] directions)
    {
        PhaseName = Name;
        SpawnSpeed = speed;
        BlockSpeed = bs;
        DoubleSidedSpawnChance = chanceOfDoubleSpawn;
        ObstacleChance = obst;
        AllowedDirectoins = directions;
    }   
}

public class PhaseManager : Singleton<PhaseManager>
{
    public delegate void OnNewPhase(PhaseDetails phaseDetails);
    public static OnNewPhase NewPhase;

    public int newPhaseBy;

    int phasesUsed = 0;
    List<PhaseDetails> pastPhases = new List<PhaseDetails>();


    // Start is called before the first frame update
    public void PhaseInit()
    {
        GeneralManager.RequestNewPhase += GetPhase;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void GetPhase(int score)
    {
        if (score % newPhaseBy == 0 || score == 0)
        {
            PhaseDetails newPhase = ChoosePhase();
            //find way to call choosephase again if Getphase matches past phases

            ImplementPhase(newPhase);

            pastPhases.Add(newPhase);
            phasesUsed++;
        }        
    }

    void ImplementPhase(PhaseDetails phase)
    {
        //Implement phase var in objspawner
        NewPhase.Invoke(phase);
    }

    PhaseDetails ChoosePhase()
    {
        PhaseDetails newPhase = Phases.Instance.AllPhases[Random.Range(0, Phases.Instance.AllPhases.Length)];

        return newPhase;
    }
}
