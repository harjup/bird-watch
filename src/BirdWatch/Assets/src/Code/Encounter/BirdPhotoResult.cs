using UnityEngine;
using System.Collections;

public class BirdPhotoResult : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    
    void Awake()
    {
        _spriteRenderer = transform.FindChild("bird-result-sprite").GetComponent<SpriteRenderer>();
    }

    public void SetSprite(string id)
    {
        var sprite = Resources.Load<Sprite>("bird-pictures/" + id.ToUpper() + "-Idle-Day");
        _spriteRenderer.sprite = sprite;
    }
}
