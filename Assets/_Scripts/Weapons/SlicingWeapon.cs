using UnityEngine;
using EzySlice;
using System.Collections.Generic;
using System;
using System.Net;

public class SlicingWeapon : WeaponModel
{

    public event Action<SlicableMesh, SlicableMesh> OnSliceDone;

    [Header("Values")]
    [SerializeField] private float cutForce = 2000f;

    public List<SlicableMesh> cannotSlice { get; private set; } = new();

    //public Transform arrow;

    //public Transform plane;
    //public Transform startPoint;

    public void CheckSlice(SlicableMesh mesh)
    {
        if (!cannotSlice.Contains(mesh))
        {
            //Slice object if not to small
            if (VolumeOfMesh(mesh.mesh) > 0.15f)
            {
                Slice(mesh);
            }
        }
    }

    public override void Effect(AttackCoord attackCoord)
    {
        base.Effect(attackCoord);
        cannotSlice.Clear();
    }

    public void Slice(SlicableMesh mesh)
    {
        Vector3 point = weapon.transform.position + weapon.transform.forward;

        //Vector3 directionToWeaponY = transform.position - weapon.transform.position;


        Vector3 finalPoint = GetLocalXY(point, transform.position, weapon.transform);

        //directionToWeaponY = weapon.transform.InverseTransformDirection(directionToWeaponY);
        //directionToWeaponY.x = directionToWeaponY.z = 0;

        //Vector3 downDir = weapon.transform.TransformDirection(directionToWeaponY);

        //Vector3 finalPoint = point + downDir;




        Vector3 planeNormal = Vector3.Cross(weapon.transform.forward, attackCoord.Direction(weapon.transform).normalized);
        planeNormal.Normalize();


        SlicedHull hull = mesh.gameObject.Slice(finalPoint, planeNormal);

        if (hull != null)
        {

            GameObject upperHull = hull.CreateUpperHull(mesh.gameObject, mesh.meshRenderer.material);
            upperHull.transform.position = mesh.transform.position;
            upperHull.transform.rotation = mesh.transform.rotation;
            SlicableMesh upperSlice = upperHull.AddComponent<SlicableMesh>();
            upperSlice.SetUpSlicableObject(ParentManager.instance.meshes, cutForce);
            cannotSlice.Add(upperSlice);

            GameObject lowerHull = hull.CreateLowerHull(mesh.gameObject, mesh.meshRenderer.material);
            lowerHull.transform.position = mesh.transform.position;
            lowerHull.transform.rotation = mesh.transform.rotation;
            SlicableMesh lowerSlice = lowerHull.AddComponent<SlicableMesh>();
            lowerSlice.SetUpSlicableObject(ParentManager.instance.meshes, cutForce);
            cannotSlice.Add(lowerSlice);

            Destroy(mesh.gameObject);
            SliceDone(lowerSlice, upperSlice);
        }
    }

    public override void AttackDone()
    {
        base.AttackDone();
        cannotSlice.Clear();
    }
    public void SliceDone(SlicableMesh slicable, SlicableMesh slicable2)
    {
        OnSliceDone?.Invoke(slicable, slicable2);
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

    private Vector3 GetLocalXY(Vector3 point, Vector3 point2, Transform parent)
    {
        Vector3 globalDirection = point2 - parent.position;

        globalDirection = parent.InverseTransformDirection(globalDirection);
        globalDirection.z = 0;

        Vector3 downDir = parent.TransformDirection(globalDirection);

        return point + downDir;
    }
}
