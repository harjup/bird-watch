using UnityEngine;
using System.Collections;
using DG.Tweening;

public class ParallaxScroller : MonoBehaviour
{

    public float StartPosition;
    public float EndPosition;
    public float Speed = 1f;

    private Tweener _tween;

    // Use this for initialization
//    void Awake()
//    {
//        Setup();
//    }

    public void Setup()
    {
        transform.localPosition = transform.localPosition.SetX(StartPosition);
        var moveTarget = transform.position.SetX(EndPosition);
        

        _tween = transform.DOMove(moveTarget, Speed)
            .SetEase(Ease.Linear)
            .SetSpeedBased()
            .SetLoops(-1, LoopType.Restart);
    }

    public void Resume()
    {
        _tween.Play();
    }

    public void Pause()
    {
        _tween.Pause();
    }
}
