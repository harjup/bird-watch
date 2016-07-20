using UnityEngine;
using System.Collections;

public class WilsonSnipeSnapshot : SnapshotBird, ISnapshotBird
{
    void Start()
    {
        // Moves ziggy zaggy along a river bank. Need a river bank graphic.
        // On picture taken they can move left / right out of frame, then keep ziggy zagging.
    }

    public void OnPictureTaken()
    {

    }
}
