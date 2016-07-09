using System;
using UnityEngine;
using System.Collections;
using Yarn.Unity;

public class EncounterRunner : MonoBehaviour
{

    private ActionSelect _actionSelect;

    // TODO: Determine if this is where we wanna store this info
    public int BirdAgitation;

    void Start ()
	{
	    var bird = EncounterStarter.Instance.Bird;

        BirdAgitation = 20;

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

    public IEnumerator RunMinigame<T>() where T: MonoBehaviour, IEncounterMinigame
    {
        yield return StartCoroutine(_actionSelect.Disable());

        MinigameResult result = null;
        var target = FindObjectOfType<T>();

        yield return StartCoroutine(target.Run((res) => { result = res; }));

        var runner = FindObjectOfType<DialogueRunner>();
        switch (result.Status)
        {
            case MinigameResult.StatusCode.Cancelled:
                yield return StartCoroutine(runner.StartAwaitableDialogue("Too_Agitated"));
                break;
            case MinigameResult.StatusCode.Success:
                

                if (target is CameraMinigame)
                {
                    // Move to outro, exit early
                    StartCoroutine(PhotoTaken());
                    yield break;
                }
                
                yield return StartCoroutine(runner.StartAwaitableDialogue("Effect_Good"));
                break;
            case MinigameResult.StatusCode.Fail:
                yield return StartCoroutine(runner.StartAwaitableDialogue("Effect_Bad"));
                break;
        }

        yield return StartCoroutine(_actionSelect.Enable());

        yield return null;
    }


    public IEnumerator PhotoTaken()
    {
        var runner = FindObjectOfType<DialogueRunner>();

        // SHOW PICTURE
        GameObject.Find("photo-result").transform.position = Vector3.zero;

        yield return new WaitForSeconds(3f);
        
        yield return StartCoroutine(runner.StartAwaitableDialogue("Battle_Exit"));

        FindObjectOfType<LevelLoader>().LoadLevel(Level.Field);

        yield return null;
    }


}
