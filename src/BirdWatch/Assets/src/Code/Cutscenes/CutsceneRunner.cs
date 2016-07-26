﻿using UnityEngine;
using System.Collections;
using Yarn.Unity;

public class CutsceneRunner : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Run());
    }

    public IEnumerator Run()
    {
        var progress = GameProgress.Instance;

        var dialogRoot = "Intermission_";

        var dayIndex = progress.CurrentDay.ToString("00");
        var currentNode = dialogRoot + dayIndex;

        var runner = FindObjectOfType<DialogueRunner>();

        yield return StartCoroutine(runner.StartAwaitableDialogue(currentNode));

        if (progress.IsEndOfFinalDay())
        {
            FindObjectOfType<TextMesh>().text = "THIS IS END FOR NOW";
            yield break;
        }
        
        progress.NextDay();
        
        LevelLoader.Instance.LoadLevel(Level.Field);
    }
}
