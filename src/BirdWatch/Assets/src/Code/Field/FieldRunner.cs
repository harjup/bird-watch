using UnityEngine;
using System.Collections;
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
            new ShowTextEvent("Field-Day-01"),
            new ShowTextEvent("Field-Day-02"), 
            new BirdEncounterEvent("Bird-Encounter-01")
        };
        
        while (true)
        {
            yield return new WaitForSeconds(2f);
            var runner = FindObjectOfType<DialogueRunner>();
            BackgroundScroller.Pause();

            var current = nodes[index];

            // TODO: Should this be in the yarn script instead?
            if (current is ShowTextEvent)
            {
                yield return StartCoroutine(runner.StartAwaitableDialogue(current.StartNode));
            }
            else if (current is BirdEncounterEvent)
            {
                yield return StartCoroutine(runner.StartAwaitableDialogue(current.StartNode));
                EncounterStarter.Instance.Init("acorn-woodpecker");
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
