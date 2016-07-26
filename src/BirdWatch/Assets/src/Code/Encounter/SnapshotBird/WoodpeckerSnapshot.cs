using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class WoodpeckerSnapshot : SnapshotBird, ISnapshotBird
{
    private List<Transform> _availablePositions;
    private Vector3 _currentLocation;

    private BirdSprite _birdSprite;

    

    private void Start()
    {
        var positionsObj = GameObject.Find("bird-fly-positions");
        _availablePositions = positionsObj.transform.Cast<Transform>().ToList();

        _birdSprite = GetComponentInChildren<BirdSprite>();

        _currentLocation = transform.localPosition;

        OnPictureTaken(true);
    }

    public void OnPictureTaken()
    {
        OnPictureTaken(false);
    }

    public void OnPictureTaken(bool force)
    {
        var distanceFromDestination = (transform.position - _currentLocation).sqrMagnitude;

        if (distanceFromDestination > .5f && !force)
        {
            return;
        }

        var nextPosition = _availablePositions
            .Select(p => p.localPosition)
            .Where(p => p != _currentLocation)
            .OrderBy(p => Guid.NewGuid())
            .First();

        var offTree = transform.localPosition + new Vector3(-.5f, -.5f, 0f);

        var pathNodes = new[] { offTree, nextPosition };

        _birdSprite.Fly();

        transform
            .DOLocalPath(pathNodes, 6f, PathType.CatmullRom, PathMode.Sidescroller2D)
            .SetLookAt(lookAhead: .5f)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(_birdSprite.Land);

        _currentLocation = nextPosition;
    }
}
