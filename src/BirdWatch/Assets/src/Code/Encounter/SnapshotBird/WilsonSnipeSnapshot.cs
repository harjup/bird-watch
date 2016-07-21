using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Random = UnityEngine.Random;

public class WilsonSnipeSnapshot : SnapshotBird, ISnapshotBird
{
    private List<Transform> _availablePositions;
    private Vector3 _currentLocation;

    public void Start()
    {
        // Moves ziggy zaggy along a river bank. Need a river bank graphic.
        // On picture taken they can move left / right out of frame, then keep ziggy zagging.

        var positionsObj = GameObject.Find("bird-fly-positions");
        _availablePositions = positionsObj.transform.Cast<Transform>().ToList();

        _currentLocation = transform.localPosition;

        OnPictureTaken();
    }

    public void OnPictureTaken()
    {
        var distanceFromDestination = (transform.localPosition - _currentLocation).sqrMagnitude;

        if (distanceFromDestination > .1f)
        {
            return;
        }

        transform.DOKill();

        // Move to next node:
        // Randomly pick left or right
        // Get closest node
        // Move there.
        // On picture taken, jump 2 nodes over.
        Debug.Log("OnPictureTaken WS");

        var direction = Random.Range(0, 2) == 0 ? -1 : 1;
        if (_currentLocation.x < -8)
        {
            direction = 1;
        }

        if (_currentLocation.x > 8)
        {
            direction = -1;
        }

        Vector3 nextPosition;
        if (direction == 1)
        {
            nextPosition = _availablePositions
                        .Select(p => p.localPosition)
                        .Where(p => p != _currentLocation)
                        .Where(p => p.x > _currentLocation.x)
                        .OrderBy(p => p.x - _currentLocation.x)
                        .First();
        }
        else
        {
            nextPosition = _availablePositions
                        .Select(p => p.localPosition)
                        .Where(p => p != _currentLocation)
                        .Where(p => p.x < _currentLocation.x)
                        .OrderByDescending(p => p.x - _currentLocation.x)
                        .First();
        }


        var dist = Mathf.Abs(_currentLocation.x - nextPosition.x);

        var zigzagDown = _currentLocation + new Vector3(direction * dist * (1f / 2f), .5f, 0f);
        var zigzagUp = _currentLocation + new Vector3(direction * dist * (6f / 8f), -.5f, 0f);

        DOTween
            .Sequence()
            .Append(transform.DOLocalMove(zigzagDown, .25f/4f).SetEase(Ease.Linear))
            .AppendInterval(Random.Range(.25f, .5f))
            .Append(transform.DOLocalMove(zigzagUp, .25f).SetEase(Ease.Linear))
            .AppendInterval(Random.Range(.25f, .5f))
            .Append(transform.DOLocalMove(nextPosition, .25f).SetEase(Ease.Linear))
            .AppendInterval(Random.Range(.2f, .3f));
            //.AppendCallback(OnPictureTaken);

        _currentLocation = nextPosition;
    }
}
