using System;
using UnityEngine;
using System.Collections;

public interface IEncounterMinigame
{
    IEnumerator Run(Action<MinigameResult> callback);
}

public class MinigameResult
{
    public static MinigameResult Fail()
    {
        return new MinigameResult(StatusCode.Fail);
    }

    public static MinigameResult Success()
    {
        return new MinigameResult(StatusCode.Success);
    }

    public static MinigameResult Cancelled()
    {
        return new MinigameResult(StatusCode.Cancelled);
    }


    public enum StatusCode
    {
        Cancelled,
        Success,
        Fail
    }


    public StatusCode Status { get; private set; }

    public MinigameResult(StatusCode status)
    {
        Status = status;
    }
}

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



    public IEnumerator Run(Action<decimal> callback)
    {
        // Easter Egg: If you hold your breath for 1 minute you pass out and the day is over??

        transform.position = Vector3.zero;

        decimal result = 0m;
        yield return StartCoroutine(FindObjectOfType<BreathBar>().Run(i => { result = i; }));

        transform.position = Vector3.zero.SetY(-20);

        callback(result);
    }
}
