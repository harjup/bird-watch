using UnityEngine;
using System.Collections;

public class SnapshotBird : MonoBehaviour
{
    private void Start()
    {
        OnPictureTaken();
    }

    public void OnPictureTaken()
    {
        var upperLeft = GameObject.Find("upper-left").transform.position;
        var lowerRight = GameObject.Find("lower-right").transform.position;

        var x = Random.Range(upperLeft.x, lowerRight.x);
        var y = Random.Range(upperLeft.y, lowerRight.y);

        transform.position = new Vector3(x, y, 0f);
    }
}
