using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BackgroundScroll : MonoBehaviour
{

    public enum BackgroundType
    {
        Day,
        Night,
        Rain
    }

    private Tweener _tween;
    
    public void Pause()
    {
        _tween.Pause();
    }

    public void Resume()
    {
        _tween.Play();
    }

    public void SetupBackground(GameObject bgPrefab)
    {
        var bg = Instantiate(bgPrefab, transform.position, Quaternion.identity) as GameObject;

        bg.transform.parent = transform;

        // night
        // rain

        // -13.5
        var moveTarget = transform.position.SetX(-11f);

        _tween = transform.DOMove(moveTarget, 1f)
            .SetEase(Ease.Linear)
            .SetSpeedBased()
            .SetLoops(-1, LoopType.Restart);
    }
}
