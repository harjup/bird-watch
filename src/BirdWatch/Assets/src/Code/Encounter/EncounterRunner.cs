using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.src.Code.Models;
using DG.Tweening;
using Yarn.Unity;

public class EncounterRunner : MonoBehaviour
{

    private ActionSelect _actionSelect;

    // TODO: Determine if this is where we wanna store this info
    public Agitation Agitation;
    
    private Bird _bird;
    
    private int _birdShots = 0;
    private int _birdShotMax = 10;

    private AudioSource _music;
    //private AudioSource _mainLoop;
    //private AudioSource _nightLoop;
    //private AudioSource _rainLoop;
    private AudioSource _victoryStart;
    private AudioSource _victoryLoop;

    private void Start()
    {
        _bird = EncounterStarter.Instance.Bird;
        
        var audioSources = transform.GetComponentsInChildren<AudioSource>();

        _victoryStart = audioSources.First(a => a.name == "music-victory-start");
        _victoryLoop = audioSources.First(a => a.name == "music-victory-loop");


        var flashLight = GameObject.Find("flash-light").GetComponent<SpriteRenderer>();
        var nightOverlay = GameObject.Find("night-overlay").GetComponent<SpriteRenderer>();
        var rainOverlay = GameObject.Find("rain-overlay").GetComponent<SpriteRenderer>();
        var day = GameProgress.Instance.CurrentDay;

        flashLight.enabled = false;
        nightOverlay.enabled = false;
        rainOverlay.enabled = false;

        // TODO: Determine how we want to inject the current metadata for testing
        if (_bird == null)
        {
            //BirdListing.GetNextDayBird();
            //BirdListing.GetNextDayBird();
            BirdListing.GetNextDayBird();
            BirdListing.GetNextDayBird();
            _bird = new Bird("CM", Bird.EncounterBg.River).At(Day.TimeOfDay.Rain);
            //_bird = new Bird("NS");
        }

        var nightForeground = GameObject.Find("foreground-night").GetComponent<SpriteRenderer>();
        var rainForeground = GameObject.Find("foreground-rain").GetComponent<SpriteRenderer>();


        nightForeground.enabled = false;
        rainForeground.enabled = false;

        if (_bird.Time == Day.TimeOfDay.Day)
        {
            _music = audioSources.First(a => a.name == "music-main-loop-day");
        }

        if (_bird.Time == Day.TimeOfDay.Night)
        {
            nightOverlay.enabled = true;
            flashLight.enabled = true;
            nightForeground.enabled = true;
            
            _birdShotMax = 15;

            _music = audioSources.First(a => a.name == "music-main-loop-night");
        }

        if (_bird.Time == Day.TimeOfDay.Rain)
        {
            rainOverlay.enabled = true;
            rainForeground.enabled = true;

            //flashLight.enabled = true;
            _birdShotMax = 20;

            _music = audioSources.First(a => a.name == "music-main-loop-rain");
        }

        _music.Play();

        FindObjectOfType<PolaroidBox>().InitializePolaroids(_birdShotMax);
        FindObjectOfType<EncounterBackground>().SetBackground(_bird.Background);
        

        Agitation = new Agitation(2.0m, 1.0m);

        FindObjectOfType<StatusMeter>().UpdateStatus(Agitation);


        _actionSelect = FindObjectOfType<ActionSelect>();

        // TODO: Acquire in a safer manner??
        FindObjectOfType<BirdBattleSprite>().SetSprite(_bird.Id);
        FindObjectOfType<BirdPhotoResult>().SetSprite(_bird);

        StartCoroutine(RunIntro());
    }

    private IEnumerator RunIntro()
    {
        var runner = FindObjectOfType<DialogueRunner>();
        FindObjectOfType<BreathBar>().enabled = false;

        yield return SceneFadeInOut.Instance.StartScene();

        // Run through any special bird messages.
        yield return StartCoroutine(runner.StartAwaitableDialogue(_bird.GetNode("Start")));

        if (GameProgress.Instance.ShowEncounterTutorial)
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue("Encounter_Tutorial"));
            GameProgress.Instance.ShowEncounterTutorial = false;
        }

        // Show menu.
        yield return StartCoroutine(_actionSelect.Enable()); //TODO: This should have a callback with the player's selection

        // Do bird thing
    }

    public IEnumerator RunCameraMinigame()
    {
        yield return StartCoroutine(_actionSelect.Disable());
        
        var runner = FindObjectOfType<DialogueRunner>();

        
        if (!Agitation.IsWithinCameraThresold())
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue("Too_Agitated"));
        }


        if (GameProgress.Instance.ShowCameraTutorialText)
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue("Camera_Tutorial"));
        }
        
        //MinigameResult result = null;
        var target = FindObjectOfType<CameraMinigame>();
        
        CameraMinigameResult result = null;
        yield return StartCoroutine(target.Run(_bird, _birdShots, _birdShotMax, Agitation.GetShakeLevel(), res => { result = res; }));
        _birdShots = result.PhotosTaken;

        if (_birdShots >= _birdShotMax)
        {
            // Move to outro, exit early
            StartCoroutine(FinalPhotoTaken());
            yield break;
        }

        Agitation.Decrement(0.5m);

        var agitationRating = Agitation.GetDescriptionNode();

   
        if (agitationRating == "AG_LEAVE")
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));
            _music.DOFade(0f, .25f);
            yield return SceneFadeInOut.Instance.EndScene();
            FindObjectOfType<LevelLoader>().LoadLevel(Level.Field);
            yield break;
        }


        yield return StartCoroutine(runner.StartAwaitableDialogue("Camera_NotEnoughShots"));
        
        FindObjectOfType<StatusMeter>().UpdateStatus(Agitation);
        
        yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));

        // TODO: Find a better method of communicating bird behavior
        //yield return StartCoroutine(runner.StartAwaitableDialogue(_bird.GetNode(agitationRating)));

        if (GameProgress.Instance.ShowCameraTutorialText)
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue("Post_Camera_Tutorial"));
            GameProgress.Instance.ShowCameraTutorialText = false;
        }

        yield return StartCoroutine(_actionSelect.Enable());

    }

    
    public IEnumerator RunBreathingMinigame()
    {
        yield return StartCoroutine(_actionSelect.Disable());
        
        var runner = FindObjectOfType<DialogueRunner>();
        if (Agitation.IsAtBestValue())
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue("Too_Calm"));
            yield return StartCoroutine(_actionSelect.Enable());
            yield break;
        }

        var showBreathingTutorial = GameProgress.Instance.ShowBreathingTutorialText;
        if (showBreathingTutorial)
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue("Breathing_Tutorial"));
            GameProgress.Instance.ShowBreathingTutorialText = false;
        }

        decimal result = 0m;
        var target = FindObjectOfType<BreathingMinigame>();

        yield return StartCoroutine(target.Run(showBreathingTutorial, res => { result = res; }));

        Agitation.Increment(result);
        
        var agitationRating = Agitation.GetDescriptionNode();
        
        if (agitationRating == "AG_LEAVE")
        {
            yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));
            _music.DOFade(0f, .25f);
            yield return SceneFadeInOut.Instance.EndScene();
            FindObjectOfType<LevelLoader>().LoadLevel(Level.Field);
            yield break;
        }
        
        FindObjectOfType<StatusMeter>().UpdateStatus(Agitation);

        yield return StartCoroutine(runner.StartAwaitableDialogue(agitationRating));

        // TODO: Find a better method of communicating bird behavior
        //yield return StartCoroutine(runner.StartAwaitableDialogue(_bird.GetNode(agitationRating)));

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
            _music.DOFade(0f, .25f);
            yield return SceneFadeInOut.Instance.EndScene();
            FindObjectOfType<LevelLoader>().LoadLevel(Level.Field);
            yield break;
        }

        yield return StartCoroutine(_actionSelect.Enable());
    }

    public IEnumerator FinalPhotoTaken()
    {
        _music.Stop();
        
        var runner = FindObjectOfType<DialogueRunner>();

        // SHOW PICTURE
        GameObject.Find("photo-result").transform.position = Vector3.zero;

        yield return StartCoroutine(ScreenFlash.Instance.Flash(2f, 1f, 1f));

        //TODO: Improve timing?
        _victoryStart.Play();
        _victoryLoop.PlayDelayed(_victoryStart.clip.length);

        yield return new WaitForSeconds(1f);
        
        yield return StartCoroutine(runner.StartAwaitableDialogue(_bird.GetBattleExitNode(runner)));

        _victoryLoop.DOFade(0f, .25f);
        yield return SceneFadeInOut.Instance.EndScene();
        FindObjectOfType<LevelLoader>().LoadLevel(Level.Field);

        yield return null;
    }

    public IEnumerator RunBookMinigame()
    {
        yield return StartCoroutine(_actionSelect.Disable());

        var runner = FindObjectOfType<DialogueRunner>();
        
        yield return StartCoroutine(runner.StartAwaitableDialogue(_bird.GetNode("Book")));

        yield return StartCoroutine(_actionSelect.Enable());

    }
}
