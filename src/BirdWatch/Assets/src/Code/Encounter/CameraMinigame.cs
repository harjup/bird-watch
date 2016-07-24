using System;
using UnityEngine;
using System.Collections;
using System.Linq;
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
    private GameObject _fieldObject;

    private AudioSource _cameraHit;
    private AudioSource _cameraHitFinal;
    private AudioSource _cameraMiss;

    private void Setup(Bird bird)
    {
        _timeLimit = GameObject.Find("time-limit");
        var prefab = Resources.Load<GameObject>("Prefabs/Snapshot/snapshot-bird-" + bird.Id);
        _fieldObject = Instantiate(prefab);
        _fieldObject.transform.parent = transform;
        _fieldObject.transform.localPosition = Vector3.zero;

        var audioSources = transform.FindChild("sfx").GetComponentsInChildren<AudioSource>();
        _cameraHit = audioSources.First(a => a.name == "sfx-camera-hit");
        _cameraHitFinal = audioSources.First(a => a.name == "sfx-camera-hit-final");
        _cameraMiss = audioSources.First(a => a.name == "sfx-camera-miss");


    }

    public IEnumerator Run(Bird bird, int birdShots, int birdShotMax, int shakeLevel, Action<CameraMinigameResult> callback)
    {
        Setup(bird);
        
        yield return new WaitForSeconds(.01f); // Wait a tiny bit so we can get set up

        // Setup
        transform.position = Vector3.zero;

        var timeLimit = 0f;
        var timeLimitMax = 6f;

        var cameraCooldown = false;

        var snapshotCamera = FindObjectOfType<SnapshotCameraDisplay>();
        snapshotCamera.LoopShake(shakeLevel);

        FindObjectsOfType<SnapshotCollider>().ToList().ForEach(s => s.Init());

        while (true)
        {
            yield return StartCoroutine(FindObjectOfType<SnapshotMover>().Run());
            
            if (Input.GetKeyDown(KeyCode.Mouse0) && !cameraCooldown)
            {
                cameraCooldown = true;
                
                var colliders = FindObjectsOfType<SnapshotCollider>();
                var okCollider = colliders
                    .First(c => c.name == "ok-cone");

                var isWithinBounds = colliders
                    .Where(c => c.name == "camera-bounds")
                    .All(c => !c.BirdInCollider);

                if (okCollider.BirdInCollider && isWithinBounds)
                {
                    birdShots += okCollider.Birds.Count;

                    if (birdShots >= birdShotMax)
                    {
                        _cameraHitFinal.Play();
                        callback(new CameraMinigameResult(birdShots));
                        break;
                    }

                    _cameraHit.Play();
                    StartCoroutine(ScreenFlash.Instance.Flash());

                    okCollider.Birds.Cast<ISnapshotBird>().ToList().ForEach(b =>
                    {
                        b.OnPictureTaken();
                        FindObjectOfType<PolaroidBox>().SpawnPicture(b.transform);
                    });

                    if (bird.Id == "BT")
                    {
                        okCollider.Birds.Clear();
                    }

                }
                else
                {
                    _cameraMiss.Play();
                    snapshotCamera.Shake();
                }

                snapshotCamera.SetInactive();
                
                StartCoroutine(StartTimer(.25f, () =>
                {
                    cameraCooldown = false;
                    snapshotCamera.SetActive();
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

        Destroy(_fieldObject);
    }

    private IEnumerator StartTimer(float seconds, Action callback)
    {
        yield return new WaitForSeconds(seconds);
        callback();
    }

}
