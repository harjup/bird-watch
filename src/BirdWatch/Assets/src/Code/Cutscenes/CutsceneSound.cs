using UnityEngine;
using System.Collections;
using Yarn.Unity;

public class CutsceneSound : MonoBehaviour
{
    [YarnCommand("play")]
    public void Play()
    {
        GetComponent<AudioSource>().Play();
    }
}
