using System;
using UnityEngine;

public class ModelContainer : MonoBehaviour
{
    public event Action<SlicableObject, SlicableObject> OnSliceDone;

    [Header("References")]
    [SerializeField] protected Transform endSlicePoint;

    protected bool isSlicable;

    protected Vector3 lastPos, newPos;
    protected Vector3 direction;
    protected virtual void Awake()
    {
        isSlicable = false;
    }
    protected virtual void Update()
    {
        newPos = endSlicePoint.position;
        direction = newPos - lastPos;
        lastPos = endSlicePoint.position;
    }
    public virtual void CheckSlice(SlicableObject slicable)
    {

    }
    public void SliceDone(SlicableObject slicable, SlicableObject slicable2)
    {
        OnSliceDone?.Invoke(slicable, slicable2);
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
    public bool IsSlicable()
    {
        return isSlicable;
    }

}
