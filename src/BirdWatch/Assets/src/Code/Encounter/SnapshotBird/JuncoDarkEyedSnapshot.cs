using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class JuncoDarkEyedSnapshot : SnapshotBird, ISnapshotBird
{
    private List<Transform> _availablePositions;
    private Vector3 _currentLocation;
    private bool _atGroundPosition;

    public void Start()
    {
        var positionsObj = GameObject.Find("bird-fly-positions");
        _availablePositions = positionsObj.transform.Cast<Transform>().ToList();

        _currentLocation = _availablePositions
            .Select(p => p.localPosition)
            .Where(p => p != _currentLocation)
            .ToList()
            .GetRandom();

        transform.localPosition = _currentLocation;

        _atGroundPosition = true;

        OnPictureTaken();
    }

    public void OnPictureTaken()
    {
        if (!_atGroundPosition)
        {
            return;
        }

        transform.DOKill();

        var nextPosition = _availablePositions
            .Select(p => p.localPosition)
            .Where(p => p != _currentLocation)
            .ToList()
            .GetRandom();
        
        var distance = (_currentLocation - nextPosition).magnitude;

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

        _atGroundPosition = false;
    }
}
