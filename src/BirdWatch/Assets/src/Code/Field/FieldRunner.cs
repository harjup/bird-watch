using UnityEngine;
using System.Collections;
using Yarn.Unity;

public class FieldRunner : MonoBehaviour
{
    private enum FieldEvent 
    {
        Unknown,
        Talk,
        Encounter
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

        var nodes = new[] 
        {
            "Field-Day-01",
            "Field-Day-02",
            "Bird-Encounter-01"
        };


        while (true)
        {
            yield return new WaitForSeconds(2f);
            var runner = FindObjectOfType<DialogueRunner>();
            BackgroundScroller.Pause();
            yield return StartCoroutine(runner.StartAwaitableDialogue(nodes[index]));
            BackgroundScroller.Resume();
            
            index++;
            if (index > 2)
            {
                index = 0;
            }


        }
    }
}
