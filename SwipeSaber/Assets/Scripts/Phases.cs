using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Phases : Singleton<Phases>
{
    public PhaseDetails[] AllPhases { get; private set;} = new PhaseDetails[2];

    public void StartFill()
    {
        FillArray();
    }

    void FillArray()
    {
        AllPhases[0] = new PhaseDetails
            (
                "Fast", 
                2.5f,
                2.5f,
                .05f,
                0,
                new Direction[2]{ Direction.Right, Direction.Left}
            );

        AllPhases[1] = new PhaseDetails
            (
                "Slow",
                1.5f,
                1.5f,
                .05f,
                0,
                new Direction[2] { Direction.Up, Direction.Down}

            );
    }
}
