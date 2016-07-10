using UnityEngine;
using System.Collections;
using UnityEditor;

public class SnapshotMover : MonoBehaviour
{
    public IEnumerator Run()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            StartCoroutine(ScreenFlash.Instance.Flash());
        }

        var mousePostion = Input.mousePosition;
        var targetPos = Camera.main.ScreenToWorldPoint(mousePostion);

        var angle = AngleBetweenVector2(transform.position, targetPos);

        
        if (angle > 180)
        {
            angle = 360 - angle;
        }
        
        if (angle > 10f)
        {
            angle = 10f;
        }
        else if (angle < -10f)
        {
            angle = -10f;
        }

        var newRotation =  transform.rotation.eulerAngles.SetZ(angle);

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
