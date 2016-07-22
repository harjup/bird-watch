using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class ActionSelect : MonoBehaviour
{
    private GameObject BreathingButton;
    private GameObject CameraButton;

    public bool Disabled = false;

    public IEnumerator Enable()
    {
        Disabled = false;
        yield return transform.DOMoveY(0f, .5f).SetEase(Ease.OutBack).WaitForCompletion();
    }

    public IEnumerator Disable()
    {
        Disabled = true;
        yield return transform.DOMoveY(-5f, .5f).SetEase(Ease.InBack).WaitForCompletion();
    }
}
