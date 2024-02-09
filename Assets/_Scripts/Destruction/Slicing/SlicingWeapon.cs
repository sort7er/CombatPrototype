using UnityEngine;
using EzySlice;
using System.Collections.Generic;
using System;

public class SlicingWeapon : WeaponModel
{

    public event Action<SlicableMesh, SlicableMesh> OnSliceDone;

    [Header("Values")]
    [SerializeField] private float cutForce = 2000f;

    [Header("References")]
    [SerializeField] protected Transform startPoint;
    [SerializeField] protected Transform endPoint;

    public List<SlicableMesh> cannotSlice { get; private set; } = new();

    public void CheckSlice(SlicableMesh sliceble, HitBox hitbox)
    {
        if (!cannotSlice.Contains(sliceble))
        {
            //Slice object if not to small
            if (VolumeOfMesh(sliceble.mesh) > 0.15f)
            {
                Slice(sliceble, hitbox);
            }
        }
    }

    public void Slice(SlicableMesh target, HitBox hitBox)
    {
        Vector3 planeNormal = Vector3.Cross(endPoint.position - startPoint.position, direction);
        planeNormal.Normalize();

        Debug.Log(1);

        SlicedHull hull = target.gameObject.Slice(endPoint.position, planeNormal);

        if(hull != null)
        {
            Debug.Log(2);
            GameObject upperHull = hull.CreateUpperHull(target.gameObject, target.meshRenderer.material);
            upperHull.transform.position = target.transform.position;
            upperHull.transform.rotation = target.transform.rotation;
            SlicableMesh upperSlice = upperHull.AddComponent<SlicableMesh>();
            upperSlice.SetUpSlicableObject(ParentManager.instance.meshes, cutForce);
            cannotSlice.Add(upperSlice);

            GameObject lowerHull = hull.CreateLowerHull(target.gameObject, target.meshRenderer.material);
            lowerHull.transform.position = target.transform.position;
            lowerHull.transform.rotation = target.transform.rotation;
            SlicableMesh lowerSlice = lowerHull.AddComponent<SlicableMesh>();
            lowerSlice.SetUpSlicableObject(ParentManager.instance.meshes, cutForce);
            cannotSlice.Add(lowerSlice);

            Destroy(target.gameObject);
            SliceDone(lowerSlice, upperSlice);
        }
    }
    public void SliceDone(SlicableMesh slicable, SlicableMesh slicable2)
    {
        OnSliceDone?.Invoke(slicable, slicable2);
    }
    public void SwingDone()
    {
        cannotSlice.Clear();
    }
    public float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }
        return Mathf.Abs(volume);
    }
    private float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }
}
