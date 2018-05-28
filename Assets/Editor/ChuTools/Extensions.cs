using UnityEngine;

public static class Extensions
{
    public static void MoveDown(this Rect rect, int amount)
    {
        rect.position = new Vector2(rect.x, rect.y + amount);
    }

    public static void SetX(this Vector3 v3, int value)
    {
        v3 = new Vector3(value, v3.y, v3.z);
    }

    public static void SetY(this Vector3 v3, int value)
    {
        v3 = new Vector3(v3.x, value, v3.z);
    }

    public static void SetZ(this Vector3 v3, int value)
    {
        v3 = new Vector3(v3.x, v3.y, value);
    }
}