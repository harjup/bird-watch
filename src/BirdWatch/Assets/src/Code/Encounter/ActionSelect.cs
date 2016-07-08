using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEditor;

public class ActionSelect : MonoBehaviour
{
    public IEnumerator Enable()
    {
        yield return transform.DOMoveY(0f, .5f).SetEase(Ease.OutBack).WaitForCompletion();
    }

    public IEnumerator Disable()
    {
        yield return transform.DOMoveY(-10f, .5f).SetEase(Ease.InBack).WaitForCompletion();
    }
}
