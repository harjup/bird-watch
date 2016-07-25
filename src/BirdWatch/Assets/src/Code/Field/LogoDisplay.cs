using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class LogoDisplay : MonoBehaviour
{
    private SpriteRenderer _logo;
    private SpriteRenderer _teamLogo;

    void Start()
    {
        var logos = GetComponentsInChildren<SpriteRenderer>();

        _logo     = logos.First(l => l.name == "logo");
        _teamLogo = logos.First(l => l.name == "team-logo");

        _logo.enabled = false;
        _teamLogo.enabled = false;
    }

    public IEnumerator RunLogos()
    {
        yield return new WaitForSeconds(3f);

        HideAll();
        ShowTeam();
        _teamLogo.color = Color.white.SetAlpha(0f);
        yield return StartCoroutine(FadeSpriteIn(_teamLogo));
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FadeSprite(_teamLogo));

        yield return new WaitForSeconds(1f);

        HideAll();
        
        ShowLogo();
        _logo.color = Color.white.SetAlpha(0f);
        yield return StartCoroutine(FadeSpriteIn(_logo));
        yield return new WaitForSeconds(4f);
        yield return StartCoroutine(FadeSprite(_logo));


        HideAll();

        yield return new WaitForSeconds(1f);

        yield return null;
    }


    private void ShowTeam()
    {
        _teamLogo.enabled = true;


        //_teamLogo.

        //_teamLogo.enabled = true;
    }

    private void ShowLogo()
    {
        _logo.enabled = true;
    }

    private void HideAll()
    {
        _teamLogo.enabled = false;
        _logo.enabled = false;
    }

    IEnumerator FadeSpriteIn(SpriteRenderer sprite)
    {
        var totalTime = .5f;
        DOTween.ToAlpha(() => sprite.color, c => sprite.color = c, 1f, totalTime);

        yield return new WaitForSeconds(totalTime);

        yield return null;
    }

    IEnumerator FadeSprite(SpriteRenderer sprite)
    {
        var totalTime = .5f;
        DOTween.ToAlpha(() => sprite.color, c => sprite.color = c, 0f, totalTime);

        yield return new WaitForSeconds(totalTime);

        yield return null;
    }
}
