using UnityEngine;
using System.Collections;
using Assets.src.Code;
using DG.Tweening;

public class CutsceneFadeSprite : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    [AwaitableYarnCommand("fade")]
    public IEnumerator Fade()
    {
        yield return StartCoroutine(FadeSprite(_spriteRenderer));
    }

    IEnumerator FadeSprite(SpriteRenderer sprite)
    {
        var totalTime = .5f;
        DOTween.ToAlpha(() => sprite.color, c => sprite.color = c, 0f, totalTime);

        yield return new WaitForSeconds(totalTime);

        yield return null;
    }
}
