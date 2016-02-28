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

    public Quaternion LocalRotation
    {
        get
        {
            return transform.localRotation;
        }
        set
        {
            transform.rotation = value;
        }
    }

    public Vector3 LocalScale
    {
        get
        {
            return transform.localScale;
        }
        set
        {
            transform.localScale = value;
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

    public void SetActive(bool active)
    {
        gameObject.SetActive(active);
    }
}
