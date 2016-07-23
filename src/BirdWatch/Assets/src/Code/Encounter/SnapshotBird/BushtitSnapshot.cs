using UnityEngine;
using System.Collections;
using DG.Tweening;

public class BushtitSnapshot : SnapshotBird, ISnapshotBird
{
    private BirdSprite _sprite;

    void Start()
    {
        _sprite = GetComponentInChildren<BirdSprite>();

        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(Random.Range(0f, .25f));
        var signOne = Random.Range(0, 2) == 0 ? -1 : 1;
        var signTwo = Random.Range(0, 2) == 0 ? -1 : 1;
        var signThree = Random.Range(0, 2) == 0 ? -1 : 1;

        var speed = Random.Range(.25f, .5f);

        transform
            .DOBlendableLocalMoveBy(Vector3.zero.SetX(.25f * signOne).SetY(.25f * signTwo), speed / 4f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);


        transform
            .DOBlendableLocalMoveBy(Vector3.zero.SetX(.5f * signThree), speed)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);

        transform.DOBlendableLocalMoveBy(Vector3.zero.SetY(-.25f * signOne), speed / 2f)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Yoyo);
    }


    public void OnPictureTaken()
    {
        _sprite.Fly();

        GetComponentInChildren<CircleCollider2D>().enabled = false;

        DOTween.Kill(gameObject);

        transform
            .DOLocalMove(new Vector3(0f, 10f, 0f), 5f)
            .SetSpeedBased();
    }
}
