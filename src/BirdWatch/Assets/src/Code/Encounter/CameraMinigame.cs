using System;
using UnityEngine;
using System.Collections;

using Yarn.Unity;

public class CameraMinigame : MonoBehaviour, IEncounterMinigame
{
    public int MaxAgitation = 50;



    private void Setup()
    {
        
    }

    public IEnumerator Run(Action<MinigameResult> callback)
    {
        // TODO: Get this in a better way.
        var agitation = FindObjectOfType<EncounterRunner>().BirdAgitation;

        // Show dialog if we can't do that
        if (agitation >= MaxAgitation)
        {
            var runner = FindObjectOfType<DialogueRunner>();
            yield return StartCoroutine(runner.StartAwaitableDialogue("Too_Agitated"));

            callback(MinigameResult.Cancelled());
            yield break;
        }



        // Setup
        transform.position = Vector3.zero;


        // Main
        var birdTimer = 0f;
        var birdTimerMax = 2f;

        var timeLimit = 0f;
        var timeLimitMax = 6f;

        while (true)
        {
            yield return StartCoroutine(FindObjectOfType<SnapshotMover>().Run());

            var birdCollision = FindObjectOfType<SnapshotCollider>().BirdInCollider;
            if (birdCollision)
            {
                birdTimer += Time.smoothDeltaTime;
            }
            else
            {
                birdTimer -= Time.smoothDeltaTime * 2f;
            }

            if (birdTimer > birdTimerMax)
            {
                callback(MinigameResult.Success());
                break;
            }

            timeLimit += Time.smoothDeltaTime;

            if (timeLimit > timeLimitMax)
            {
                callback(MinigameResult.Fail());
                break;
            }
        }

        Cleanup();

        
    }

    private void Cleanup()
    {
        transform.position = Vector3.zero.SetY(-20);
    }

}
