using UnityEngine;

public interface ISnapshotBird
{
    void OnPictureTaken();

    // I'm using this to expose the gameobject's transform on the inferface.
    // Kinda janky, though it is nice to only get gameobject properties I want
    Transform transform { get; }
}
