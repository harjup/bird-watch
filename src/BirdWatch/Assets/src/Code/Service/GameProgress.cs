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
    public readonly int FinalDay = 3;

    public bool ShowEncounterTutorial = false;
    public bool ShowCameraTutorialText = false;
    public bool ShowBreathingTutorialText = false;


    private void Awake()
    {
        ResetEncounterCount();
    }

    public void StartEncounter()
    {
        EncounterCount++;
    }

    public void NextDay()
    {
        CurrentDay++;
        ResetEncounterCount();
    }

    public bool IsEndOfFinalDay()
    {
        return CurrentDay >= FinalDay;
    }


    public void ResetEncounterCount()
    {
        EncounterCount = 0;
    }
}
