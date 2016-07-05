using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BackgroundScroll : MonoBehaviour
{
    private Tweener Tween;

    void Start()
    {
        // -13.5
        var moveTarget = Vector3.zero.SetX(-13.5f);

        Tween = transform.DOMove(moveTarget, 1f)
            .SetEase(Ease.Linear)
            .SetSpeedBased()
            .SetLoops(-1, LoopType.Restart);
    }


    public void Pause()
    {
        Tween.Pause();
    }

    public void Resume()
    {
        Tween.Play();
    }
}
