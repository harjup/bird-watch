using UnityEngine;
using System.Collections;

public class SnapshotCollider : MonoBehaviour
{
    public bool BirdInCollider;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        var isBird = collision.GetComponentInParent<SnapshotBird>() != null;
        if (isBird)
        {
            BirdInCollider = true;
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        var isBird = collision.GetComponentInParent<SnapshotBird>() != null;
        if (isBird)
        {
            BirdInCollider = false;
        }
    }
}
