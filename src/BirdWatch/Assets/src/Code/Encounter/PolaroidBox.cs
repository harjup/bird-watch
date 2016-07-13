using UnityEngine;
using DG.Tweening;

public class PolaroidBox : MonoBehaviour
{
    private GameObject _polaroid;

    private Vector3 _lowerRight;

    private Vector3 _spawnLocation;

    void Start()
    {
        _polaroid = Resources.Load<GameObject>("Prefabs/Battle/Polaroid");
    }

    public void SpawnPicture()
    {

        var upperRight = transform.FindChild("box-bg").localPosition;
        _lowerRight = transform.FindChild("polaroid-lower-right").localPosition;

        _spawnLocation = FindObjectOfType<SnapshotMover>().transform.position;

        var result = Instantiate(_polaroid, _spawnLocation, Quaternion.identity) as GameObject;

        result.transform.parent = transform.parent;

        var target = new Vector3(
            Random.Range(upperRight.x, _lowerRight.x),
            Random.Range(upperRight.y, _lowerRight.y),
            0f);

        result
            .transform
            .DOLocalPath(new [] { target } , .25f, PathType.CatmullRom, PathMode.Sidescroller2D)
            .SetEase(Ease.OutSine);
    }
}
