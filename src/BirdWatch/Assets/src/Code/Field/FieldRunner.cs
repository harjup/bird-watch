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

    IEnumerator WalkTheField()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            var runner = FindObjectOfType<DialogueRunner>();
            BackgroundScroller.Pause();
            yield return StartCoroutine(runner.StartAwaitableDialogue("Example_Encounter"));
            BackgroundScroller.Resume();
        }
    }



}
