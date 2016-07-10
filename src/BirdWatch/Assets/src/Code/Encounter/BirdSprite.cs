using UnityEngine;
using System.Collections;

public class BirdSprite : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;


    // Use this for initialization
    void Start()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public void SetSprite(string id)
    {
        var sprite = Resources.Load<Sprite>("bird-sprites/" + id.ToUpper() + "-BattleSprite");
        _spriteRenderer.sprite = sprite;
    }


}
