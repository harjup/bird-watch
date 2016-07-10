using UnityEngine;
using System.Collections;

public class BirdBattleSprite : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public void SetSprite(string id)
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var sprite = Resources.Load<Sprite>("bird-sprites/" + id.ToUpper() + "-BattleSprite");
        _spriteRenderer.sprite = sprite;
    }


}
