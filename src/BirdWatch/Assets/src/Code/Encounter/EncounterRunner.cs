using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.src.Code.Encounter;
using Yarn.Unity;

public class EncounterRunner : MonoBehaviour
{

    private ActionSelect _actionSelect;

    // TODO: Determine if this is where we wanna store this info
    public int BirdAgitation;


    // TODO: Need to figure out a better way to associate a number range with a given status
    List<int> _agitationLevels = new List<int> {20, 50, 70, 90};


    public void ApplyBreatheResult(Bar.Type res)
    {
        switch (res)
        {
            case Bar.Type.Good:
                BirdAgitation -= 5;
                break;
            case Bar.Type.Ok:
                BirdAgitation -= 3;
                break;
            case Bar.Type.Bad:
            case Bar.Type.Clear:
                BirdAgitation += 5;
                break;
        }
    }


    private int RateAgitation(int current)
    {
        for (int i = 0; i < _agitationLevels.Count; i++)
        {
            var val = _agitationLevels[i];
            if (current <= val)
            {
                return i;
            }
        }

        return -1;
    }

    private void Start()
    {
        var bird = EncounterStarter.Instance.Bird;

        BirdAgitation = 20;

        _actionSelect = FindObjectOfType<ActionSelect>();

        Debug.Log("RUNNING ENCOUNTER FOR BIRD " + bird);
        StartCoroutine(RunIntro());
    }

    private IEnumerator RunIntro()
    {
        var runner = FindObjectOfType<DialogueRunner>();
        FindObjectOfType<BreathBar>().enabled = false;

        // Run through any special bird messages.
        yield return StartCoroutine(runner.StartAwaitableDialogue("AW_Start"));

        // Show menu.
        yield return StartCoroutine(_actionSelect.Enable());

        // Do bird thing
    }

    public IEnumerator RunMinigame<T>() where T : MonoBehaviour, IEncounterMinigame
    {
        yield return StartCoroutine(_actionSelect.Disable());

        MinigameResult result = null;
        var target = FindObjectOfType<T>();

        yield return StartCoroutine(target.Run(res => { result = res; }));


        // TODO: Make this a dictionary or something I dunno
        var agitationRating = RateAgitation(BirdAgitation);
        string flavorText = "AG_OK";
        switch (agitationRating)
        {
            case 0:
                //Great
                flavorText = "AG_GREAT";
                break;
            case 1:
                //Ok
                flavorText = "AG_OK";
                break;
            case 2:
                // Meh
                flavorText = "AG_BAD";
                break;
            case 3:
                // Bad
                flavorText = "AG_MAD";
                break;
            case -1:
                //TERRIBLE!!!
                flavorText = "AG_RAGE";
                break;
        }


        var runner = FindObjectOfType<DialogueRunner>();

        yield return StartCoroutine(runner.StartAwaitableDialogue(flavorText));

        // TODO: Extract this into something nicer. Each Minigame should handle its own results scripting stuff.
        // TODO: Figure out how we can propagate breaking out of this workflow when we end the cycle with a successful camera shot.

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
        // END EXTRACT LOGIC ---------------------------------


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
