using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class LogoDisplay : MonoBehaviour
{
    private SpriteRenderer _logo;
    private SpriteRenderer _logoBack;
    private SpriteRenderer _teamLogo;

    private AudioSource _cardFlipAudio;

    void Start()
    {
        var logos = GetComponentsInChildren<SpriteRenderer>();

        _logo     = logos.First(l => l.name == "logo");
        _logoBack = logos.First(l => l.name == "logo-back");
        _teamLogo = logos.First(l => l.name == "team-logo");

        _cardFlipAudio = GetComponent<AudioSource>();


        _logo.transform.localPosition = _logo.transform.localPosition.SetX(30f);
        _logoBack.transform.localPosition = _logoBack.transform.localPosition.SetX(30f);

        //_logo.enabled = false;
        _teamLogo.enabled = false;
    }

    public IEnumerator RunLogos()
    {
        yield return new WaitForSeconds(3f);

//        HideAll();
//        ShowTeam();
//        _teamLogo.color = Color.white.SetAlpha(0f);
//        yield return StartCoroutine(FadeSpriteIn(_teamLogo));
//        yield return new WaitForSeconds(4f);
//        yield return StartCoroutine(FadeSprite(_teamLogo));

//        yield return new WaitForSeconds(1f);

//        HideAll();

        yield return 
            DOTween.Sequence()
            .Append(_logo.transform.DOLocalMoveX(13.4f, .5f))
            .AppendInterval(4f)
            .AppendCallback(() => { _cardFlipAudio.Play(); })
            .Append(_logo.transform.DOLocalMoveX(30f, .5f))
            .Append(_logoBack.transform.DOLocalMoveX(13.4f, .5f))
            .AppendInterval(5f)
            .Append(_logoBack.transform.DOLocalMoveX(30f, .5f))
            .WaitForCompletion();

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
