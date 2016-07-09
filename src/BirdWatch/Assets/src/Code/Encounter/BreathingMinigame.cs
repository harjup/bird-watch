using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BreathingMinigame : MonoBehaviour
{
    public void Start()
    {
        //Enable();
    }

    public void RenderBar()
    {
        // 0 -----------------------------  100
        // Bad size -> 20
        // Ok size -> 10
        // Good size -> 5
    }



    public void Enable()
    {
        transform.position = Vector3.zero;
        FindObjectOfType<BreathBar>().enabled = true;


        //transform.DOShakePosition(100f, .25f, 5).SetLoops(-1, LoopType.Restart);

        // Easter Egg: If you hold your breath for 1 minute you pass out and the day is over??

        // When mouse held -> bar size increases
        // When mouse up   -> bar size decreases

        // Alternate face between inhale / exhale
        // Need good / bad indicators (color, shakes?)

        // When mouse state changes:
        //   Check the position of the bar.
        //   Rank the range.
        //   Apply to bird's calmness.
    }
}
