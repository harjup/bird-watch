using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class SnapshotCollider : MonoBehaviour
{
    public bool BirdInCollider { get { return Birds.Any(); } }
    
    public readonly List<SnapshotBird> Birds = new List<SnapshotBird>();

    public void Init()
    {
        Birds.Clear(); 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var bird = collision.GetComponentInParent<SnapshotBird>();
        if (!Birds.Contains(bird))
        {
            Birds.Add(bird);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        var bird = collision.GetComponentInParent<SnapshotBird>();
        if (Birds.Contains(bird))
        {
            Birds.Remove(bird);
        }
    }
}
