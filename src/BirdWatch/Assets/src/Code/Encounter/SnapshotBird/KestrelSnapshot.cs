using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DG.Tweening;

public class KestrelSnapshot : SnapshotBird, ISnapshotBird
{
    private List<Transform> _leftPositions;
    private List<Transform> _rightPositions;
    private List<Transform> _centerPositions;
    private Vector3 _currentLocation;

    private void Start()
    {
        var positionsObj = GameObject.Find("bird-fly-positions");
        var positions = positionsObj.transform.Cast<Transform>().ToList();
        _leftPositions = positions.Where(p => p.name == "left-side").ToList();
        _rightPositions = positions.Where(p => p.name == "right-side").ToList();
        _centerPositions = positions.Where(p => p.name == "center").ToList();

        _currentLocation = transform.localPosition;

        transform.localPosition = Vector3.zero.SetX(8.6f);

        Swoop();
    }

    public void Swoop()
    {
        Vector3 nextPosition;
        if (transform.localPosition.x < 0)
        {
            nextPosition = _rightPositions
                .Select(p => p.localPosition)
                .OrderBy(p => Guid.NewGuid())
                .First();
        }
        else
        {
            nextPosition = _leftPositions
                .Select(p => p.localPosition)
                .OrderBy(p => Guid.NewGuid())
                .First();
        }

        var center = _centerPositions
            .Select(p => p.localPosition)
            .OrderBy(p => Guid.NewGuid())
                .First();


        var pathNodes = new[] { center, nextPosition };
        transform
            .DOLocalPath(pathNodes, 6f, PathType.CatmullRom, PathMode.Sidescroller2D)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .OnComplete(Swoop); // We want to swoop in a loop!

        _currentLocation = nextPosition;
    }

    public void OnPictureTaken()
    {
//        if (transform.position.magnitude < 8.6f)
//        {
//            return;
//        }
    }
}
