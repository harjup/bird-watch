﻿using UnityEngine;
using System.Collections;
using Yarn.Unity;

public class EncounterRunner : MonoBehaviour
{

    private ActionSelect _actionSelect;

    void Start ()
	{
	    var bird = EncounterStarter.Instance.Bird;

        _actionSelect = FindObjectOfType<ActionSelect>();

        Debug.Log("RUNNING ENCOUNTER FOR BIRD " + bird);
        StartCoroutine(RunIntro());
    }

    IEnumerator RunIntro()
    {
        var runner = FindObjectOfType<DialogueRunner>();
        FindObjectOfType<BreathBar>().enabled = false;

        // Run through any special bird messages.
        yield return StartCoroutine(runner.StartAwaitableDialogue("AW_Start"));
        
        // Show menu.
        yield return StartCoroutine(_actionSelect.Enable());

        
        
        // Do bird thing
    }

    public IEnumerator RunMinigame(EncounterMenuButton.Type buttonType)
    {
        yield return StartCoroutine(_actionSelect.Disable());

        FindObjectOfType<BreathingMinigame>().Enable();
        // Do appropriate minigame when a button gets picked


        yield return null;
    }
}
