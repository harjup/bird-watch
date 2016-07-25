using UnityEngine;
using System.Collections;

public class BirdBattleSprite : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    public void SetSprite(string id)
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();

        var prefab = Resources.Load<GameObject>("Prefabs/BirdSprites/" + id.ToUpper());

        var result = Instantiate(prefab);
        
        result.transform.parent = transform;
        result.transform.localPosition = Vector3.zero;

        //var sprite = Resources.Load<Sprite>("bird-sprites/" + id.ToUpper() + "-BattleSprite");
        //_spriteRenderer.sprite = sprite;
    }


}
