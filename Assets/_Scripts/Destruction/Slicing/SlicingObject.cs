using UnityEngine;
using EzySlice;
using System.Collections.Generic;

public class SlicingObject : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float cutForce = 2000f;

    [Header("References")]
    [SerializeField] private Transform endSlicePoint;
    [SerializeField] private Transform startSlicePoint;

    private Vector3 lastPos, newPos;
    private Vector3 direction;

    private List<SlicableObject> cannotSlice = new();


    private void Update()
    {
        newPos = endSlicePoint.position;
        direction = newPos - lastPos;
        lastPos = endSlicePoint.position;
    }

    public void Slice(SlicableObject target)
    {
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, direction);
        planeNormal.Normalize();




        SlicedHull hull = target.gameObject.Slice(endSlicePoint.position, planeNormal);

        if(hull != null)
        {
            GameObject upperHull = hull.CreateUpperHull(target.gameObject, target.meshRenderer.material);
            SlicableObject upperSlice = upperHull.AddComponent<SlicableObject>();
            upperSlice.SetUpSlicableObject(target.transform.parent, cutForce);
            cannotSlice.Add(upperSlice);

            GameObject lowerHull = hull.CreateLowerHull(target.gameObject, target.meshRenderer.material);
            SlicableObject lowerSlice = lowerHull.AddComponent<SlicableObject>();
            lowerSlice.SetUpSlicableObject(target.transform.parent, cutForce);
            cannotSlice.Add(lowerSlice);

            Destroy(target.gameObject);
        }
    }

    public void SwingDone()
    {
        cannotSlice.Clear();
    }
}
