using UnityEngine;
using System.Collections;
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

        // the area has loaded
        // pick the background & characters to scroll and set that up
        // fire off an event waiter thing
        // 5 or 10 seconds
        // either do dialog for area that hasn't been seen yet or do another bird
        StartCoroutine(WalkTheField());

    }

    // 1 -> dialog
    // 2 -> dialog
    // 3 -> bird found
    
    IEnumerator WalkTheField()
    {
        var index = 0;

        var nodes = new FieldEvent[] 
        {
            new ShowTextEvent("Day_Chat"),
            new BirdEncounterEvent("")
        };
        
        while (true)
        {
            yield return new WaitForSeconds(2f);
            var runner = FindObjectOfType<DialogueRunner>();
            BackgroundScroller.Pause();

            var current = nodes[index];


            var gameProgress = GameProgress.Instance;
            if (current is ShowTextEvent)
            {

                //End of day. Say we're going home and then cut to black where it transaitions to next day.
                if (gameProgress.EncounterCount > gameProgress.EncounterMax)
                {
                    yield return StartCoroutine(runner.StartAwaitableDialogue("End_Of_Day"));
                    LevelLoader.Instance.LoadLevel(Level.Cutscene);

                    break;
                }

                var node = current.StartNode + "_" + gameProgress.EncounterCount.ToString("00");
                
                yield return StartCoroutine(runner.StartAwaitableDialogue(node));
            }
            else if (current is BirdEncounterEvent)
            {
                var bird = BirdListing.GetNextDayBird();
                
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


        }
    }
}
