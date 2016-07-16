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
    private GameObject _timeLimit;

    private void Setup()
    {
        _timeLimit = GameObject.Find("time-limit");
    }

    public IEnumerator Run(int birdShots, int birdShotMax, Action<CameraMinigameResult> callback)
    {
        Setup();

        // Setup
        transform.position = Vector3.zero;

        var timeLimit = 0f;
        var timeLimitMax = 4f;

        var cameraCooldown = false;

        while (true)
        {
            yield return StartCoroutine(FindObjectOfType<SnapshotMover>().Run());
            
            if (Input.GetKeyDown(KeyCode.Mouse0) && !cameraCooldown)
            {
                cameraCooldown = true;
                
                var birdCollision = FindObjectOfType<SnapshotCollider>().BirdInCollider;
                if (birdCollision)
                {
                    birdShots += 1;

                    if (birdShots >= birdShotMax)
                    {
                        callback(new CameraMinigameResult(birdShots));
                        break;
                    }

                    StartCoroutine(ScreenFlash.Instance.Flash());


                    FindObjectOfType<SnapshotBird>().OnPictureTaken();
                    FindObjectOfType<PolaroidBox>().SpawnPicture();
                }

                FindObjectOfType<SnapshotCameraDisplay>().SetInactive();
                
                StartCoroutine(StartTimer(.25f, () =>
                {
                    cameraCooldown = false;
                    FindObjectOfType<SnapshotCameraDisplay>().SetActive();
                }));
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

    private IEnumerator StartTimer(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }

}
