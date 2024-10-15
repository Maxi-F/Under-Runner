using UnityEngine;

public static class Vector3Utitlities
{
    public static Vector3 IgnoreY(this Vector3 v3)
    {
        Vector3 result = v3;
        result.y = 0;
        return result;
    }
}