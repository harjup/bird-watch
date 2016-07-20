using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class StellerJaySnapshot : SnapshotBird, ISnapshotBird
{

    private List<Transform> _startingPoints;
    private List<Transform> _landingPoints;
    bool doneFlyingIn = false;

    void Start()
    {
        var positionsObj = GameObject.Find("bird-fly-positions");
        var availablePositions = positionsObj.transform.Cast<Transform>().ToList();
        

        // Starting points:
        _startingPoints = availablePositions.Where(p => p.name == "start-position").ToList();
        _landingPoints = availablePositions.Where(p => p.name == "land-position").ToList();
        
        FlyInTween();



        // Quickly swoop onto the screen, hop around.
        // On picture taken, fly off screen, pause, fly onto another spot and start hopping around.
        // Rinse and repeat.
    }


    public void FlyInTween()
    {
        var startSpot = _startingPoints.GetRandom();

        // If they start on the left they should land on the left, start right -> land right.
        // Comparing floats sucks, man
        var landingSpot = _landingPoints
            .Where(l => Math.Abs(Mathf.Sign(l.localPosition.x) - Mathf.Sign(startSpot.localPosition.x)) < .01)
            .ToList()
            .GetRandom();

        transform.localPosition = startSpot.transform.localPosition;

        // TODO: While I'd like to just have a straightforward sequence. SetSpeedBased doesn't seem to work in children in a sequence. I should file a bug with DOTween.
        transform
            .DOLocalMove(landingSpot.localPosition, 8f)
            .SetEase(Ease.Linear)
            .SetSpeedBased()
            .OnComplete(() =>
            {
                DOTween
                    .Sequence()
                    .Append(transform.DOLocalJump(transform.localPosition.AddX(1.5f), .25f, 4, 1f).SetEase(Ease.Linear))
                    .Append(transform.DOLocalJump(transform.localPosition.AddX(-.5f), .25f/2f, 2, .5f).SetEase(Ease.Linear))
                    .OnComplete(() =>
                    {
                        transform.DOLocalMove(startSpot.localPosition, 8f).SetEase(Ease.Linear)
                        .SetSpeedBased()
                        .OnComplete(FlyInTween);
                    });

            });   
    }

    public void OnPictureTaken()
    {
        //TODO: Determine if we want it to do anything when a picture is taken.
        // Maybe it should spawn a little surprised mark so you know you got them

//        if (doneFlyingIn)
//        {
//            transform.DOKill();
//            FlyInTween();
//        }
    }
}
