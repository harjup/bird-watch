using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.src.Code.Models;
using DG.Tweening;
using Yarn.Unity;

public class FieldRunner : MonoBehaviour
{
    private class ShowTextEvent : FieldEvent
    {
        public ShowTextEvent(string startNode) 
            : base(startNode) {}
    }

    private class BirdEncounterEvent : FieldEvent
    {
        public BirdEncounterEvent(string startNode) 
            : base(startNode) {}
    }

    private abstract class FieldEvent
    {
        public FieldEvent(string startNode)
        {
            StartNode = startNode;
        }

        public string StartNode { get; private set; }
        
    }

    private BackgroundScroll BackgroundScroller;

    private List<AudioSource> _activeAudioSources = new List<AudioSource>();

    void Awake()
    {
        BackgroundScroller = FindObjectOfType<BackgroundScroll>();
        
        var dayNumber = GameProgress.Instance.CurrentDay;
        var day = new DayListing().GetDay(dayNumber);


        SetupAudio(day);
        SetupVisuals(day);


        // the area has loaded
        // pick the background & characters to scroll and set that up
        // fire off an event waiter thing
        // 5 or 10 seconds
        // either do dialog for area that hasn't been seen yet or do another bird
        StartCoroutine(WalkTheField(day));

    }

    private void SetupVisuals(Day day)
    {
        var fieldBackground = day.GetFieldBackground();
        BackgroundScroller.SetupBackground(fieldBackground);
    }

    private void SetupAudio(Day day)
    {
        if (day.Time == Day.TimeOfDay.Day)
        {
            _activeAudioSources = transform.FindChild("music-daytime").GetComponents<AudioSource>().ToList();
        }

        if (day.Time == Day.TimeOfDay.Night)
        {
            _activeAudioSources = transform.FindChild("music-night").GetComponents<AudioSource>().ToList();
        }

        if (day.Time == Day.TimeOfDay.Rain)
        {
            _activeAudioSources = transform.FindChild("music-rain").GetComponents<AudioSource>().ToList();
        }
        
        _activeAudioSources.ForEach(a => a.Play());
    }

    private void FadeAudio()
    {
        foreach (var activeAudioSource in _activeAudioSources)
        {
            activeAudioSource.DOFade(0f, .5f);
        }
    }

    // 1 -> dialog
    // 2 -> dialog
    // 3 -> bird found
    
    IEnumerator WalkTheField(Day day)
    {
        yield return SceneFadeInOut.Instance.StartScene();

        var index = 0;

        var nodes = new FieldEvent[] 
        {
            new ShowTextEvent("Intro"),
            new ShowTextEvent("Day_Chat"),
            new BirdEncounterEvent("")
        };

        while (true)
        {
            var runner = FindObjectOfType<DialogueRunner>();
            
            var current = nodes[index];


            var gameProgress = GameProgress.Instance;
            if (current is ShowTextEvent)
            {

                //End of day. Say we're going home and then cut to black where it transaitions to next day.
                if (gameProgress.EncounterCount >= gameProgress.EncounterMax)
                {
                    if (day.Time == Day.TimeOfDay.Day)
                    {
                        yield return StartCoroutine(runner.StartAwaitableDialogue("End_Of_Day"));
                    }

                    if (day.Time == Day.TimeOfDay.Night)
                    {
                        yield return StartCoroutine(runner.StartAwaitableDialogue("End_Of_Night"));
                    }

                    if (day.Time == Day.TimeOfDay.Rain)
                    {
                        yield return StartCoroutine(runner.StartAwaitableDialogue("End_Of_Rain"));
                    }

                    FadeAudio();
                    yield return SceneFadeInOut.Instance.EndScene();

                    LevelLoader.Instance.LoadLevel(Level.Cutscene);

                    break;
                }

                // TODO: Pretty terrible if I do say so myself. Make less terrible.
                if (current.StartNode == "Intro")
                {
                    if (gameProgress.EncounterCount == 0
                        && gameProgress.CurrentDay == 1)
                    {
                        BackgroundScroller.Pause();
                        yield return StartCoroutine(runner.StartAwaitableDialogue("Day_Chat_Intro"));

                        BackgroundScroller.Resume();

                        yield return  StartCoroutine(FindObjectOfType<LogoDisplay>().RunLogos());
                    }
                }
                else
                {
                    //var node = current.StartNode + "_" + .ToString("00");
                    var node = day.DialogBetweenEncounters[gameProgress.EncounterCount];


                    yield return StartCoroutine(runner.StartAwaitableDialogue(node));
                }
            }
            else if (current is BirdEncounterEvent)
            {
                BackgroundScroller.Pause();

                //var bird = BirdListing.GetNextDayBird();
                var bird = day.AvailableBirds[gameProgress.EncounterCount];


                var approachNode = bird.GetNode("Approach");
                
                yield return StartCoroutine(runner.StartAwaitableDialogue(approachNode));

                yield return StartCoroutine(runner.StartAwaitableDialogue("Bird_Found"));

                gameProgress.StartEncounter();

                FadeAudio();
                yield return SceneFadeInOut.Instance.EndScene();

                EncounterStarter.Instance.Init(bird);

                break;
            }
            
            BackgroundScroller.Resume();
            
            index++;
            if (index > 2)
            {
                index = 0;
            }

            yield return new WaitForSeconds(2f);
        }
    }
}
