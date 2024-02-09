using System;
using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] protected Transform endSlicePoint;


    protected Vector3 lastPos, newPos;
    protected Vector3 direction;

    protected virtual void Update()
    {
        newPos = endSlicePoint.position;
        direction = newPos - lastPos;
        lastPos = endSlicePoint.position;
    }

    public Vector3 Position()
    {
        return transform.position;
    }
    public Vector3 Direction()
    {
        return direction;
    }
    public Vector3 UpDir()
    {
        return transform.up;
    }

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

}
