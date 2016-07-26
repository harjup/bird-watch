using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;


public class SwoopySnapshotBird : SnapshotBird, ISnapshotBird
{
    private List<Transform> _availablePositions;

    private void Start()
    {
        var positionsObj = GameObject.Find("bird-fly-positions");
        _availablePositions = positionsObj.transform.Cast<Transform>().ToList();

        //OnPictureTaken();
    }

    private Vector3 _currentLocation;


    public void OnPictureTaken()
    {
        var nextPositions = _availablePositions
            .Select(p => p.localPosition)
            .Where(p => p != _currentLocation)
            .OrderBy(p => Guid.NewGuid())
            .ToList();
        
        var pathNodes = nextPositions.Take(2).ToArray();
        _currentLocation = pathNodes.Last();


        transform
            .DOLocalPath(pathNodes, 5f, PathType.CatmullRom, PathMode.Sidescroller2D)
            .SetSpeedBased()
            .SetEase(Ease.OutSine);
    }
   
}
