using UnityEngine;
using System.Collections;

public static class AngleCalc
{
    public static float AngleInRadians(Vector3 vec1, Vector3 vec2)
    {
        return Mathf.Atan2(vec2.y - vec1.y, vec2.x - vec1.x);
    }

    public static float AngleInDegrees(Vector3 vec1, Vector3 vec2)
    {
        return AngleInRadians(vec1, vec2) * 180 / Mathf.PI;
    }
}
