﻿using UnityEngine;
using System.Collections;
using Yarn.Unity;

public class EncounterStarter : Singleton<EncounterStarter>
{
    public string Bird = "None";
    
    public void Init(string bird)
    {
        Bird = bird;
        LevelLoader.Instance.LoadLevel(Level.Encounter);
    }

    IEnumerator RunIntro()
    {
        var runner = FindObjectOfType<DialogueRunner>();
        yield return StartCoroutine(runner.StartAwaitableDialogue("AW_Start"));
        EncounterMenuGui.Instance.Enable();
    }
}
