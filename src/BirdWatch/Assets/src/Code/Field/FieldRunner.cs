﻿using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Linq;
using Assets.src.Code.Models;
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
            transform.FindChild("music-daytime").GetComponents<AudioSource>().ToList().ForEach(a => a.Play());
        }

        if (day.Time == Day.TimeOfDay.Night)
        {
            transform.FindChild("music-night").GetComponents<AudioSource>().ToList().ForEach(a => a.Play());
        }

        if (day.Time == Day.TimeOfDay.Rain)
        {
            transform.FindChild("music-rain").GetComponents<AudioSource>().ToList().ForEach(a => a.Play());
        }
    }

    // 1 -> dialog
    // 2 -> dialog
    // 3 -> bird found
    
    IEnumerator WalkTheField(Day day)
    {
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
                    yield return StartCoroutine(runner.StartAwaitableDialogue("End_Of_Day"));
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
                EncounterStarter.Instance.Init(bird);
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
