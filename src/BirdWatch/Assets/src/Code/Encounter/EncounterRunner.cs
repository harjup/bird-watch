using UnityEngine;
using System.Collections;
using Yarn.Unity;

public class EncounterRunner : MonoBehaviour
{
    
	void Start ()
	{
	    var bird = EncounterStarter.Instance.Bird;
        Debug.Log("RUNNING ENCOUNTER FOR BIRD " + bird);
        StartCoroutine(RunIntro());
    }

    IEnumerator RunIntro()
    {
        var runner = FindObjectOfType<DialogueRunner>();
        yield return StartCoroutine(runner.StartAwaitableDialogue("AW_Start"));
        EncounterMenuGui.Instance.Enable();
    }
}
