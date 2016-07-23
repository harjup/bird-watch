using UnityEngine;
using System.Collections;
using DG.Tweening;

public class SnapshotMover : MonoBehaviour
{
    public float TurnRadius = 30f;

    public IEnumerator Run()
    {
        var mousePostion = Input.mousePosition;
        var targetPos = Camera.main.ScreenToWorldPoint(mousePostion);

        var angle = AngleBetweenVector2(transform.position, targetPos);
        
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        
        yield return null;
    }

    private float AngleBetweenVector2(Vector2 vec1, Vector2 vec2)
    {
        Vector2 diference = vec2 - vec1;
        float sign = (vec2.y < vec1.y) ? -1.0f : 1.0f;
        return Vector2.Angle(Vector2.right, diference) * sign;
    }


}
