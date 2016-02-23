using UnityEngine;

public class MonoView : MonoBehaviour
{
    public Transform Parent
    {
        get
        {
            return transform.parent;
        }
        set
        {
            transform.parent = value;
        }
    }

    public Vector3 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public Vector3 LocalPosition
    {
        get
        {
            return transform.localPosition;
        }
        set
        {
            transform.localPosition = value;
        }
    }

    public Quaternion Rotation
    {
        get
        {
            return transform.rotation;
        }
        set
        {
            transform.rotation = value;
        }
    }

    public Vector3 Up
    {
        get
        {
            return transform.up;
        }
    }

    public Vector3 Down
    {
        get
        {
            return -Up;
        }
    }

    public Vector3 Forward
    {
        get
        {
            return transform.forward;
        }
    }
}
