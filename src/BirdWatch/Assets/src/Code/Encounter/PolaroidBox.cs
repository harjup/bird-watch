using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

public class PolaroidBox : MonoBehaviour
{
    private GameObject _polaroid;

    private Vector3 _lowerRight;

    private Vector3 _spawnLocation;

    public int PhotoMax = 10;

    private List<Vector3> _polaroidLocations = new List<Vector3>();

    public void InitializePolaroids(int count)
    {
        _polaroid = Resources.Load<GameObject>("Prefabs/Battle/Polaroid");

        var upperRight = transform.FindChild("box-bg").localPosition;
        for (int i = 0; i < count; i++)
        {
            var spawnLocation = upperRight.AddX(i * 0.5f).AddY(-.25f);
            _polaroidLocations.Add(spawnLocation);

            var result = Instantiate(_polaroid, Vector3.zero, Quaternion.identity) as GameObject;
            result.transform.parent = transform.parent;
            result.transform.localPosition = spawnLocation;
            result.GetComponent<SpriteRenderer>().color = Color.grey.SetAlpha(.5f);
        }

    }

    public void SpawnPicture(Transform birdTransform)
    {

        var upperRight = transform.FindChild("box-bg").localPosition;
        _lowerRight = transform.FindChild("polaroid-lower-right").localPosition;

        //_spawnLocation = FindObjectOfType<SnapshotMover>().transform.position;
        
        var result = Instantiate(_polaroid, birdTransform.position.AddZ(-3f), Quaternion.identity) as GameObject;

        result.transform.parent = transform.parent;
        

        //Possible async problem?
        var target = _polaroidLocations.First().AddZ(-.5f);
        _polaroidLocations.RemoveAt(0);

//        var target = new Vector3(
//            Random.Range(upperRight.x, _lowerRight.x),
//            Random.Range(upperRight.y, _lowerRight.y),
//            0f);

        result
            .transform
            .DOLocalPath(new [] { target } , .5f, PathType.CatmullRom, PathMode.Sidescroller2D)
            .SetEase(Ease.OutSine);
    }
}
