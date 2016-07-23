using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class NorthernSawWhetSnapshot : SnapshotBird, ISnapshotBird
{
    private List<Transform> _availablePositions;
    private Vector3 _currentLocation;
    private BirdSprite _birdSprite;

    private void Start()
    {
        var positionsObj = GameObject.Find("bird-fly-positions");
        _availablePositions = positionsObj.transform.Cast<Transform>().ToList();

        _currentLocation = transform.localPosition;

        _birdSprite = GetComponentInChildren<BirdSprite>();

        OnPictureTaken();
    }

    public void OnPictureTaken()
    {
        var distanceFromDestination = (transform.position - _currentLocation).sqrMagnitude;

        if (distanceFromDestination > .25f)
        {
            return;
        }

        transform.DOKill();

        var nextPosition = _availablePositions
            .Select(p => p.localPosition)
            .Where(p => p != _currentLocation)
            .OrderBy(p => Guid.NewGuid())
            .First();

        var offTree = transform.localPosition + new Vector3(0f, .5f, 0f);

        _birdSprite.Fly();

        var pathNodes = new[] { offTree, nextPosition };
        transform
            .DOLocalPath(pathNodes, 12f, PathType.CatmullRom, PathMode.Sidescroller2D)
            .SetLookAt(1f)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(() => { _birdSprite.Land();});

        _currentLocation = nextPosition;
    }
}
