using UnityEngine;
using System.Collections;

public class StellerJaySnapshot : SnapshotBird, ISnapshotBird
{
    void Update()
    {
        // Quickly swoop onto the screen, hop around.
        // On picture taken, fly off screen, pause, fly onto another spot and start hopping around.
        // Rinse and repeat.
    }

    public void OnPictureTaken()
    { 

    }
}
