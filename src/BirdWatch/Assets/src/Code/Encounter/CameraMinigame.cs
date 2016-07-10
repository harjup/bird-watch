using System;
using UnityEngine;
using System.Collections;

using Yarn.Unity;

public class CameraMinigameResult
{
    public int PhotosTaken { get; private set; }

    public CameraMinigameResult(int photosTaken)
    {
        PhotosTaken = photosTaken;
    }
}

public class CameraMinigame : MonoBehaviour
{
    public int MaxAgitation = 50;
    
    private GameObject _timeLimit;

    private void Setup()
    {
        _timeLimit = GameObject.Find("time-limit");
    }

    public IEnumerator Run(int birdShots, int birdShotMax, Action<CameraMinigameResult> callback)
    {
        Setup();

        // TODO: Get this in a better way.
        //var agitation = FindObjectOfType<EncounterRunner>().BirdAgitation;

        // Show dialog if we can't do that
        // TODO: This should be handled by selecting the menu option
//        if (agitation >= MaxAgitation)
//        {
//            var runner = FindObjectOfType<DialogueRunner>();
//            yield return StartCoroutine(runner.StartAwaitableDialogue("Too_Agitated"));
//
//            callback(MinigameResult.Cancelled());
//            yield break;
//        }



        // Setup
        transform.position = Vector3.zero;

        var timeLimit = 0f;
        var timeLimitMax = 4f;

        while (true)
        {
            yield return StartCoroutine(FindObjectOfType<SnapshotMover>().Run());

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                StartCoroutine(ScreenFlash.Instance.Flash());

                var birdCollision = FindObjectOfType<SnapshotCollider>().BirdInCollider;
                if (birdCollision)
                {
                    birdShots += 1;
                    FindObjectOfType<SnapshotBird>().OnPictureTaken();
                }
            }

            
            if (timeLimit > timeLimitMax)
            {
                timeLimit = timeLimitMax;
            }
            if (timeLimit < 0)
            {
                timeLimit = 0f;
            }
            
            _timeLimit.transform.localScale = _timeLimit.transform.localScale.SetX(1 - (timeLimit/timeLimitMax));

            if (birdShots >= birdShotMax)
            {
                callback(new CameraMinigameResult(birdShots));
                break;
            }

            timeLimit += Time.smoothDeltaTime;

            if (timeLimit >= timeLimitMax)
            {
                callback(new CameraMinigameResult(birdShots));
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
