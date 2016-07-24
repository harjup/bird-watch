using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BreathingFace : MonoBehaviour
{
    public Sprite In;
    public Sprite Out;

    private SpriteRenderer _renderer;

    private Vector3 _initialPosition;

    void Start ()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _initialPosition = transform.localPosition;
    }

    public void Toggle(bool isIn)
    {
        _renderer.sprite = isIn ? In : Out;
    }

    public void Shake()
    {
        transform.localPosition = _initialPosition;
        transform.DOKill();
        transform.DOShakePosition(.1f, new Vector2(.5f, .5f), 20, 45);
    }
}
