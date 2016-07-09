using System;
using UnityEngine;
using System.Collections;
using DG.Tweening;

public class FloatySprite : MonoBehaviour
{
    public enum SpriteGraphic
    {
        Unknown,
        Good,
        Ok,
        Bad
    }

    public Sprite Good;
    public Sprite Ok;
    public Sprite Bad;

    public void SetSprite(SpriteGraphic graphic)
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();

        switch (graphic)
        {
            case SpriteGraphic.Good:
                spriteRenderer.sprite = Good;
                break;
            case SpriteGraphic.Ok:
                spriteRenderer.sprite = Ok;
                break;
            case SpriteGraphic.Bad:
                spriteRenderer.sprite = Bad;
                break;
        }
    }


    // Use this for initialization
    private void Start()
    {
        transform.DOMoveY(1f, .25f).SetRelative().SetEase(Ease.OutSine).OnComplete(() => { Destroy(gameObject); });
    }
}
