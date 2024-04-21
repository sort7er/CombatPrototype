using UnityEngine;
using System;
using DynamicMeshCutter;
using System.Xml.Linq;

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

    public override void Effect()
    {
        base.Effect();
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

        MeshCreation.TranslateCreatedObjects(info, data.CreatedObjects, data.CreatedTargets, cutter.Separation);

        for (int i = 0; i < data.CreatedObjects.Length; i++)
        {
            SetUpObjects(data.CreatedObjects[i], cutForce);
        }

        for (int i = 0; i < data.CreatedTargets.Length; i++)
        {
            SetUpTargets(data.CreatedTargets[i]);
        }

    }


    public void SetUpObjects(GameObject meshObject, float cutForce = 500f)
    {
        meshObject.transform.parent = ParentManager.instance.meshes;

        Tools.SetLayerForAllChildren(meshObject, 7);

        Rigidbody rb = meshObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddExplosionForce(cutForce, attackCoord.EndPos(weapon.transform), 1);
        }

        Destroy(meshObject, 4f);
    }

    public void SetUpTargets(MeshTarget meshTarget)
    {
        if (meshTarget.TryGetComponent<MeshFilter>(out MeshFilter meshFilter))
        {
            //7 is ignoreplayer and 9 is ragdoll
            if (Tools.VolumeOfMesh(meshFilter.mesh) < collisionThreshold)
            {
                meshTarget.gameObject.layer = 7;
            }
            else
            {
                meshTarget.gameObject.layer = 9;
            }
        }
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
