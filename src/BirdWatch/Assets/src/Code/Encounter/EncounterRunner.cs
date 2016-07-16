using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.src.Code.Models;
using Yarn.Unity;

public class EncounterRunner : MonoBehaviour
{

    private ActionSelect _actionSelect;

    // TODO: Determine if this is where we wanna store this info
    public Agitation Agitation;
    

    private string RateAgitation(decimal current)
    {
        var descriptionMap = new Dictionary<int, string>()
        {
            //{3, "AG_GREAT"},
            {2, "AG_OK" },
            {1, "AG_BAD" },
            {0, "AG_LEAVE" },

        };

        var actual = (int)Math.Round(current, MidpointRounding.AwayFromZero);
        return descriptionMap[actual];
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
        
        Agitation = new Agitation(2.0m);
        FindObjectOfType<StatusMeter>().UpdateStatus(Agitation.Value);


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

        Agitation.Decrement(0.5m);

        var agitationRating = RateAgitation(Agitation.Value);

        var runner = FindObjectOfType<DialogueRunner>();

        if (agitationRating == "AG_LEAVE")
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));
            FindObjectOfType<LevelLoader>().LoadLevel(Level.Field);
            yield break;
        }


        yield return StartCoroutine(runner.StartAwaitableDialogue("Camera_NotEnoughShots"));
        
        yield return StartCoroutine(runner.StartAwaitableDialogue(_bird.GetNode(agitationRating)));

        FindObjectOfType<StatusMeter>().UpdateStatus(Agitation.Value);


        yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));

        yield return StartCoroutine(_actionSelect.Enable());

    }





    public IEnumerator RunBreathingMinigame()
    {
        yield return StartCoroutine(_actionSelect.Disable());

        decimal result = 0m;
        var target = FindObjectOfType<BreathingMinigame>();

        yield return StartCoroutine(target.Run(res => { result = res; }));

        Agitation.Increment(result);
        
        var agitationRating = RateAgitation(Agitation.Value);
        
        var runner = FindObjectOfType<DialogueRunner>();

        if (agitationRating == "AG_LEAVE")
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));
            FindObjectOfType<LevelLoader>().LoadLevel(Level.Field);
            yield break;
        }

        yield return StartCoroutine(runner.StartAwaitableDialogue(_bird.GetNode(agitationRating)));

        FindObjectOfType<StatusMeter>().UpdateStatus(Agitation.Value);

        yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));

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
