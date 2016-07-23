using UnityEngine;
using System.Collections;

public class BirdSprite : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private Sprite _flyingSprite;
    private Sprite _landedSprite;

    private void Start()
    {
        _flyingSprite = Resources.Load<Sprite>("minigame-sprite/snap-bird-wings");
        _landedSprite = Resources.Load<Sprite>("minigame-sprite/snap-bird-land");

        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Fly()
    {
        _spriteRenderer.sprite = _flyingSprite;
    }

    public void Land()
    {
        _spriteRenderer.sprite = _landedSprite;
    }

    public void Face(Vector3 position)
    {
        transform.LookAt(position);
    }
}
