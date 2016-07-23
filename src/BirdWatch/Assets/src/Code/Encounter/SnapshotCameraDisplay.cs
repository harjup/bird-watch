using UnityEngine;
using System.Collections;
using System.Linq;
using DG.Tweening;

public class SnapshotCameraDisplay : MonoBehaviour
{
    private SpriteRenderer _iconSprite;
    private SpriteRenderer _okSprite;
    private SpriteRenderer _perfectSprite;

    private Vector3 _initialPosition;

    void Start()
    {
        var sprites = GetComponentsInChildren<SpriteRenderer>();
        _iconSprite = sprites.First(s => s.name == "camera-icon");
        _okSprite = sprites.First(s => s.name == "ok-cone");

        _initialPosition = transform.localPosition;
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

    public void LoopShake(int shakeLevel)
    {
        if (shakeLevel == 0)
        {
            return;
        }
        
        float power = (.25f/8f) * shakeLevel;
        float time = .125f;
        
        var signOne = Random.Range(0, 2) == 0 ? -1 : 1;
        var signTwo = Random.Range(0, 2) == 0 ? -1 : 1;
        var signThree = Random.Range(0, 2) == 0 ? -1 : 1;

        transform
            .DOBlendableLocalMoveBy(Vector3.zero.SetX(power * signOne).SetY(power * signTwo), time / 4f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);


        transform
            .DOBlendableLocalMoveBy(Vector3.zero.SetX(power * 2 * signThree), time)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);

        transform.DOBlendableLocalMoveBy(Vector3.zero.SetY(-power * signOne), time / 2f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);

        //transform.DOShakePosition(.25f, new Vector2(.25f, .25f), 20, 45).SetLoops(-1, LoopType.Yoyo);
    }

    public void ShakeLight()
    {
        transform.DOShakePosition(.25f, new Vector2(.25f/4f, .25f/4f), 20, 45).SetLoops(-1, LoopType.Yoyo);
    }

    public void ShakeHarsh()
    {
        transform.DOShakePosition(.25f, new Vector2(.5f, .5f), 20, 45).SetLoops(-1, LoopType.Yoyo);
    }

    public void Shake()
    {
        transform.DOShakePosition(.25f, new Vector2(.25f, .25f), 20, 45);
    }

    public void KillShake()
    {
        transform.DOKill();
        transform.localPosition = _initialPosition;
    }


}
