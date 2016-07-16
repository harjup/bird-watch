using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;

public class StatusMeter : MonoBehaviour
{
    private Dictionary<decimal, float> _agitationPositions = 
        new Dictionary<decimal, float>
    {
            //{3.0m, -8.41f },
            //{2.5m, -5.53f },
            {2.0m, -2.62f },
            {1.5m, .1f },
            {1.0m, 3.6f },
            {0.5m, 5.65f },
            {0.0m, 6.02f }
    };

    private GameObject _statusSprite;

    private void Start()
    {
        _statusSprite = transform.FindChild("status-ok").gameObject;
    }


    public void UpdateStatus(decimal amount)
    {
        Debug.Log(amount);
        var val = _agitationPositions[amount];

        _statusSprite.transform.DOLocalMoveX(val, .5f).SetEase(Ease.OutBack);
    }
}
