using UnityEngine;
using System.Collections;
using Assets.src.Code.Encounter;

public class RatingBar : MonoBehaviour
{
    public Bar Bar { get; set; }


    private SpriteRenderer _renderer;
    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    public void Hide()
    {
        _renderer.enabled = false;
    }

    public void Show()
    {
        _renderer.enabled = true;
    }

    public bool IsActive()
    {
        return _renderer.enabled;
    }
}
