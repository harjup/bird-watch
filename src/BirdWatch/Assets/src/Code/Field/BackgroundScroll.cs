using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BackgroundScroll : MonoBehaviour
{
	void Start ()
    {
        // -13.5
	    var moveTarget = Vector3.zero.SetX(-13.5f);

	    transform.DOMove(moveTarget, 1f)
            .SetEase(Ease.Linear)
            .SetSpeedBased()
            .SetLoops(-1, LoopType.Restart);
    }
}
