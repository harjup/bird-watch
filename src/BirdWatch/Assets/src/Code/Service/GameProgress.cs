using UnityEngine;
using System.Collections;
using UnityEngineInternal;

public class GameProgress : Singleton<GameProgress>
{
    // 1 -> DayTime
    // 2 -> Night
    // 3 -> Raining
    public int CurrentDay = 1;
    public int EncounterCount { get; private set; }
    public readonly int EncounterMax = 5;

    private void Awake()
    {
        ResetEncounterCount();
    }

    public void StartEncounter()
    {
        EncounterCount++;
    }

    public void ResetEncounterCount()
    {
        EncounterCount = 1;
    }
}
