using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class EveningGrosbeakSnapshot : SnapshotBird, ISnapshotBird
{
    private List<Transform> _availablePositions;
    private Vector3 _currentLocation;
    private bool _atGroundPosition;

    private void Start()
    {
        var positionsObj = GameObject.Find("bird-fly-positions");
        _availablePositions = positionsObj.transform.Cast<Transform>().ToList();

        _currentLocation = transform.localPosition;

        _atGroundPosition = true;

        OnPictureTaken();
    }

    public void OnPictureTaken()
    {
        // var distanceFromDestination = (transform.position - _currentLocation).sqrMagnitude;
//        if (distanceFromDestination > .25f)
//        {
//            return;
//        }

        if (!_atGroundPosition)
        {
            return;
        }

        transform.DOKill();

        var nextPosition = _availablePositions
            .Select(p => p.localPosition)
            .Where(p => p != _currentLocation)
            .OrderBy(p => Guid.NewGuid())
            .First();
        
        var distance = (_currentLocation - nextPosition).magnitude;


        transform
                .DOLocalJump(nextPosition, .25f, (int)distance * 1, .25f * distance)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _atGroundPosition = true;
                    transform
                        .DOLocalJump(transform.localPosition.AddX(.25f), .25f/4, 1, .5f)
                        .SetLoops(-1, LoopType.Yoyo);
                });

        _atGroundPosition = false;

        /*
                transform
                    .DOLocalJump(nextPosition, .25f, (int)distance * 1, .25f * distance)
                    .SetEase(Ease.Linear)
                    .OnComplete(() =>
                    {
                        _atGroundPosition = true;
                        transform
                            .DOLocalJump(transform.localPosition.AddX(1f), .25f, 2, .5f)
                            .SetLoops(-1, LoopType.Yoyo);
                    });

                _currentLocation = nextPosition;
                */

        _currentLocation = nextPosition;
    }
}
