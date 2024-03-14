using UnityEngine;
using System;
using DynamicMeshCutter;

[RequireComponent(typeof(Cutter))]
public class SlicingWeapon : WeaponModel 
{

    public event Action<MeshCreationData> OnMeshCreated;

    [Header("Values")]
    [SerializeField] private float cutForce = 2000f;
    [SerializeField] private float volumeThreshold;

    [Header("References")]
    [SerializeField] private Cutter cutter;

    //public Transform arrow;

    //public Transform plane;
    //public Transform startPoint;

    public override void Effect(AttackCoord attackCoord)
    {
        base.Effect(attackCoord);
    }

    public void Slice(MeshTarget mesh)
    {
        Vector3 point = weapon.transform.position + weapon.transform.forward;
        Vector3 finalPoint = GetLocalXY(point, transform.position, weapon.transform);

        Vector3 planeNormal = Vector3.Cross(weapon.transform.forward, attackCoord.Direction(weapon.transform).normalized);
        planeNormal.Normalize();

        
        cutter.Cut(mesh, finalPoint, planeNormal, null, OnCreated);

        //plane.position = finalPoint;
        //plane.rotation = Quaternion.LookRotation(planeNormal);
        //plane.Rotate(90, 0, 0);
    }
    public override void AttackDone()
    {
        base.AttackDone();
    }
    private void OnCreated(Info info, MeshCreationData data)
    {
        OnMeshCreated?.Invoke(data);

        for (int i = 0; i < data.CreatedObjects.Length; i++)
        {
            SetUpSlicableObject(data.CreatedObjects[i], cutForce);
        }
    }

    private Vector3 GetLocalXY(Vector3 point, Vector3 point2, Transform parent)
    {
        Vector3 globalDirection = point2 - parent.position;

        globalDirection = parent.InverseTransformDirection(globalDirection);
        globalDirection.z = 0;

        Vector3 downDir = parent.TransformDirection(globalDirection);

        return point + downDir;
    }

    public void SetUpSlicableObject(GameObject mesh, float cutForce = 500f)
    {
        mesh.transform.parent = ParentManager.instance.meshes;

        for(int i = 0; i < mesh.transform.childCount; i++)
        {
            //if (Tools.VolumeOfMesh(mesh.transform.GetChild(i).gameObject.GetComponent<MeshFilter>().mesh) < volumeThreshold)
            //{
            //}
                mesh.transform.GetChild(i).gameObject.layer = 7;

        }

        Rigidbody rb = mesh.GetComponent<Rigidbody>();

        rb.AddExplosionForce(cutForce, transform.position, 1);
        Destroy(mesh, 4f);
    }
}
