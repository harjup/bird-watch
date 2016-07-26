using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Yarn.Unity;

public class CutsceneShowEndingImage : MonoBehaviour
{
    private GameObject _thanksForPlaying;
    private GameObject _quitButton;

    void Awake()
    {
        _thanksForPlaying = transform.FindChild("thanks-for-playing").gameObject;

        _thanksForPlaying.SetActive(false);

        _quitButton = transform.FindChild("quit-button").gameObject;

        _quitButton.SetActive(false);
    }

    [YarnCommand("show")]
    public void Show()
    {
        _thanksForPlaying.SetActive(true);
        _quitButton.SetActive(true);
    }
}
