using UnityEngine;
using System.Collections;
using DG.Tweening;

public class EncounterMinigame : MonoBehaviour
{
    public void Enable()
    {
        transform.position = Vector3.zero;
        transform.DOShakePosition(100f, .25f, 5).SetLoops(-1, LoopType.Restart);
    }
}
