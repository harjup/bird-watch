using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ActionSelect : MonoBehaviour
{
    private GameObject BreathingButton;
    private GameObject CameraButton;
    
    public IEnumerator Enable()
    {
        yield return transform.DOMoveY(0f, .5f).SetEase(Ease.OutBack).WaitForCompletion();
    }

    public IEnumerator Disable()
    {
        yield return transform.DOMoveY(-10f, .5f).SetEase(Ease.InBack).WaitForCompletion();
    }
}
