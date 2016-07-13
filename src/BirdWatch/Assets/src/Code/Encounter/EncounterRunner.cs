﻿using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.src.Code.Encounter;
using Assets.src.Code.Models;
using Yarn.Unity;

public class EncounterRunner : MonoBehaviour
{

    private ActionSelect _actionSelect;

    // TODO: Determine if this is where we wanna store this info
    public int BirdAgitation;


    // TODO: Need to figure out a better way to associate a number range with a given status
    List<int> _agitationLevels = new List<int> {20, 50, 70, 100};


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


    private string RateAgitation(int current)
    {
        var descriptionMap = new Dictionary<int, string>()
        {
            { 0, "AG_GREAT"},
            {1, "AG_OK" },
            {2, "AG_BAD" },
            {3, "AG_LEAVE" },

        };

        for (int i = 0; i < _agitationLevels.Count; i++)
        {
            var val = _agitationLevels[i];
            if (current <= val)
            {
                return descriptionMap[i];
            }
        }

        return descriptionMap[3];
    }

    

    private Bird _bird;

    private void Start()
    {
        _bird = EncounterStarter.Instance.Bird;

        // TODO: Determine how we want to inject the current metadata for testing
        if (_bird == null)
        {
            BirdListing.GetNextDayBird();
            BirdListing.GetNextDayBird();
            BirdListing.GetNextDayBird();
            _bird = BirdListing.GetCurrentDayBird();
        }
        
        BirdAgitation = 20;

        _actionSelect = FindObjectOfType<ActionSelect>();

        // TODO: Acquire in a safer manner??
        FindObjectOfType<BirdBattleSprite>().SetSprite(_bird.Id);
        FindObjectOfType<BirdPhotoResult>().SetSprite(_bird.Id);

        StartCoroutine(RunIntro());
    }

    private IEnumerator RunIntro()
    {
        var runner = FindObjectOfType<DialogueRunner>();
        FindObjectOfType<BreathBar>().enabled = false;

        // Run through any special bird messages.
        yield return StartCoroutine(runner.StartAwaitableDialogue(_bird.GetNode("Start")));

        // Show menu.
        yield return StartCoroutine(_actionSelect.Enable()); //TODO: This should have a callback with the player's selection

        // Do bird thing
    }


    private int _birdShots = 0;
    private int _birdShotMax = 10;
    public IEnumerator RunCameraMinigame()
    {
        yield return StartCoroutine(_actionSelect.Disable());

        //MinigameResult result = null;
        var target = FindObjectOfType<CameraMinigame>();
        
        CameraMinigameResult result = null;
        yield return StartCoroutine(target.Run(_birdShots, _birdShotMax, res => { result = res; }));
        _birdShots = result.PhotosTaken;

        if (_birdShots >= _birdShotMax)
        {
            // Move to outro, exit early
            StartCoroutine(FinalPhotoTaken());
            yield break;
        }
        
        BirdAgitation += 10;

        var agitationRating = RateAgitation(BirdAgitation);
        
        var runner = FindObjectOfType<DialogueRunner>();

        if (agitationRating == "AG_LEAVE")
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));
            FindObjectOfType<LevelLoader>().LoadLevel(Level.Field);
            yield break;
        }


        yield return StartCoroutine(runner.StartAwaitableDialogue("Camera_NotEnoughShots"));
        
        yield return StartCoroutine(runner.StartAwaitableDialogue(_bird.GetNode(agitationRating)));

        yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));

        yield return StartCoroutine(_actionSelect.Enable());

    }





    public IEnumerator RunMinigame<T>() where T : MonoBehaviour, IEncounterMinigame
    {
        yield return StartCoroutine(_actionSelect.Disable());

        MinigameResult result = null;
        var target = FindObjectOfType<T>();

        yield return StartCoroutine(target.Run(res => { result = res; }));

        var agitationRating = RateAgitation(BirdAgitation);
  
        var runner = FindObjectOfType<DialogueRunner>();

        if (agitationRating == "AG_LEAVE")
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));
            FindObjectOfType<LevelLoader>().LoadLevel(Level.Field);
            yield break;
        }

        yield return StartCoroutine(runner.StartAwaitableDialogue(_bird.GetNode(agitationRating)));

        yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));
        
        // TODO: Extract this into something nicer. Each Minigame should handle its own results scripting stuff.
        // TODO: Figure out how we can propagate breaking out of this workflow when we end the cycle with a successful camera shot.

        //        switch (result.Status)
        //        {
        //            case MinigameResult.StatusCode.Cancelled:
        //                yield return StartCoroutine(runner.StartAwaitableDialogue("Too_Agitated"));
        //                break;
        //            case MinigameResult.StatusCode.Success:
        //                yield return StartCoroutine(runner.StartAwaitableDialogue("Effect_Good"));
        //                break;
        //            case MinigameResult.StatusCode.Fail:
        //                yield return StartCoroutine(runner.StartAwaitableDialogue("Effect_Bad"));
        //                break;
        //        }
        // END EXTRACT LOGIC ---------------------------------


        yield return StartCoroutine(_actionSelect.Enable());

        yield return null;
    }

    public IEnumerator RunExitChoice()
    {
        yield return StartCoroutine(_actionSelect.Disable());

        var runner = FindObjectOfType<DialogueRunner>();

        yield return StartCoroutine(runner.StartAwaitableDialogue("Run_Away_Option"));

        int choice = -1;

        yield return
            StartCoroutine(FindObjectOfType<TextDisplayGui>().ShowChoicesAndWait(
                new List<string> {"Yes", "No"}, 
            i => { choice = i; }));

        Debug.Log(choice);

        if (choice == 0)
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue("Run_Away"));

            FindObjectOfType<LevelLoader>().LoadLevel(Level.Field);
            yield break;
        }

        yield return StartCoroutine(_actionSelect.Enable());
    }

    public IEnumerator FinalPhotoTaken()
    {
        var runner = FindObjectOfType<DialogueRunner>();

        // SHOW PICTURE
        GameObject.Find("photo-result").transform.position = Vector3.zero;

        yield return StartCoroutine(ScreenFlash.Instance.Flash(2f, 1f, 1f));

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(runner.StartAwaitableDialogue(_bird.GetNode("Battle_Exit")));

        FindObjectOfType<LevelLoader>().LoadLevel(Level.Field);

        yield return null;
    }
}
