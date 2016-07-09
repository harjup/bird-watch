using UnityEngine;
using System.Collections;

public class BreathingFace : MonoBehaviour
{
    public Sprite In;
    public Sprite Out;

    private SpriteRenderer _renderer;

    void Start ()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Toggle(bool isIn)
    {
        _renderer.sprite = isIn ? In : Out;
    }
}
