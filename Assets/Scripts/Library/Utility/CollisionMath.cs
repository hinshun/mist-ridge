using UnityEngine;

public static class CollisionMath
{
    public static Vector3 DownslopeDirection(Vector3 normal, Vector3 down)
    {
        return Vector3.Cross(Vector3.Cross(normal, down), normal);
    }
}
