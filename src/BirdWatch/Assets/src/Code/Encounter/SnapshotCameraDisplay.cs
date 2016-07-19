using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class SnapshotCameraDisplay : MonoBehaviour
{
    private SpriteRenderer _iconSprite;
    private SpriteRenderer _okSprite;
    private SpriteRenderer _perfectSprite;

    void Start()
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        _iconSprite = sprites.First(s => s.name == "camera-icon");
        _okSprite = sprites.First(s => s.name == "ok-cone");
    }

    public void SetActive()
    {
        _iconSprite.color = Color.white;
        _okSprite.color = Color.white.SetAlpha(.30f);
    }

    public void SetInactive()
    {
        _iconSprite.color = Color.grey;
        _okSprite.color = Color.white.SetAlpha(0f);
    }

    public void Shake()
    {
        transform.DOShakePosition(.25f, new Vector2(.25f, .25f), 20, 45);
    }
}
