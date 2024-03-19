using UnityEngine;
using System;
using DynamicMeshCutter;

[RequireComponent(typeof(Cutter))]
public class SlicingWeapon : WeaponModel 
{

    public event Action<MeshCreationData> OnMeshCreated;

    [Header("Values")]
    [SerializeField] private float cutForce = 2000f;
    [SerializeField] private float collisionThreshold;
    [SerializeField] private float cuttingThreshold;

    [Header("References")]
    [SerializeField] private Cutter cutter;

    //public Transform arrow;

    //public Transform plane;
    //public Transform startPoint;

    public override void Effect(AttackCoord attackCoord)
    {
        base.Effect(attackCoord);
    }

    public void Slice(MeshTarget meshTarget)
    {
        Mesh mesh = GetMesh(meshTarget);

        if (mesh != null)
        {
            if (!VolumeCheck(mesh))
            {
                return;
            }
        }




        Vector3 point = weapon.transform.position + weapon.transform.forward;

        //Change slicing mode depending on if its dual wield or not

        Vector3 finalPoint;
        if(weapon.currentAttack.currentWield == Attacks.Wield.both)
        {
            finalPoint = Tools.GetLocalXY(point, transform.position, weapon.transform);
        }
        else
        {
            finalPoint = Tools.GetLocalY(point, transform.position, weapon.transform);
        }

        Vector3 planeNormal = Vector3.Cross(weapon.transform.forward, attackCoord.Direction(weapon.transform).normalized);
        planeNormal.Normalize();

        cutter.Cut(meshTarget, finalPoint, planeNormal, null, OnCreated);

    }

    private void OnCreated(Info info, MeshCreationData data)
    {
        OnMeshCreated?.Invoke(data);

        for (int i = 0; i < data.CreatedObjects.Length; i++)
        {
            SetUpSlicableObject(data.CreatedObjects[i].transform, cutForce);
        }
    }



    public void SetUpSlicableObject(Transform meshParent, float cutForce = 500f)
    {
        meshParent.parent = ParentManager.instance.meshes;
        meshParent.gameObject.layer = 7;

        //for(int i = 0; i < meshParent.childCount; i++)
        //{
        //    if(meshParent.GetChild(i).TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
        //    {
        //        if (Tools.VolumeOfMesh(meshFilter.mesh) < collisionThreshold)
        //        {
        //            meshParent.GetChild(i).gameObject.layer = 7;
        //        }
        //    }
        //}

        //Rigidbody rb = meshParent.GetComponent<Rigidbody>();

        //rb.AddExplosionForce(cutForce, transform.position, 1);
        Destroy(meshParent.gameObject, 4f);
    }

    private Mesh GetMesh(MeshTarget meshTarget)
    {
        MeshFilter meshFilter = meshTarget.GetComponent<MeshFilter>();
        
        if(meshFilter != null)
        {
            return meshFilter.mesh;
        }

        SkinnedMeshRenderer skinnedMeshRenderer = meshTarget.GetComponent<SkinnedMeshRenderer>();

        if(skinnedMeshRenderer != null)
        {
            return skinnedMeshRenderer.sharedMesh;
        }
        else
        {
            return null;
        }
    }

    private bool VolumeCheck(Mesh mesh)
    {
        if (Tools.VolumeOfMesh(mesh) > cuttingThreshold)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
