using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class CommonMerganserSnapshot : SnapshotBird, ISnapshotBird
{
    private List<Transform> _availablePositions;
    private Vector3 _startLocation;
    private Vector3 _endLocation;
    private Vector3 _currentLocation;

    private void Start()
    {
        var positionsObj = GameObject.Find("bird-fly-positions");
        var positions = positionsObj.transform.Cast<Transform>().ToList();

        _startLocation = positions.First(a => a.name == "river-start").transform.localPosition;
        _endLocation = positions.First(a => a.name == "river-end").transform.localPosition;

        _availablePositions = positions.Where(a => a.name == "river-position").ToList();

        _currentLocation = transform.localPosition;

        FloatDownRiver();
    }

    private Tween _floatTween;

    public void FloatDownRiver()
    {
        var riverPoints = _availablePositions
            .Select(p => p.localPosition)
            .Where(p => p != _currentLocation)
            .OrderBy(p => p.x)
            .ToList();

        var offTree = transform.localPosition + new Vector3(-.5f, -.5f, 0f);

        var pathNodes = new[] { offTree };

        riverPoints.Insert(0, _startLocation);
        riverPoints.Add(_endLocation);

        transform.localPosition = _startLocation;

        _floatTween = transform
            .DOLocalPath(riverPoints.ToArray(), 3f, PathType.CatmullRom, PathMode.Sidescroller2D)
            .SetSpeedBased()
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart)
            .OnStepComplete(() => { _floatTween.timeScale = 1f; });

        
    }

    private Tween _sequence;
    public void OnPictureTaken()
    {
        _floatTween.timeScale = 4f;

        if (_sequence != null && _sequence.IsPlaying())
        {
            _sequence.Kill();
        }
        
        _sequence = DOTween
            .Sequence()
            .AppendInterval(.25f)
            .AppendCallback(() => { _floatTween.timeScale = 1f; });
    }
}
