using UnityEngine;
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
        var process = GameProgress.Instance;

        var dialogRoot = "Intermission_";
        var dayIndex = process.CurrentDay.ToString("00");
        var currentNode = dialogRoot + dayIndex;

        var runner = FindObjectOfType<DialogueRunner>();

        yield return StartCoroutine(runner.StartAwaitableDialogue(currentNode));

        process.CurrentDay++;

        LevelLoader.Instance.LoadLevel(Level.Field);
    }
}
