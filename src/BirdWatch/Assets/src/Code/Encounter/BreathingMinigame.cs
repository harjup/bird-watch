using UnityEngine;
using System.Collections;
using DG.Tweening;

public interface IEncounterMinigame
{
    IEnumerator Run();
}

public class BreathingMinigame : MonoBehaviour, IEncounterMinigame
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



    public IEnumerator Run()
    {
        // Easter Egg: If you hold your breath for 1 minute you pass out and the day is over??

        transform.position = Vector3.zero;
        
        yield return StartCoroutine(FindObjectOfType<BreathBar>().Run());

        transform.position = Vector3.zero.SetY(-20);
    }
}
