using DynamicMeshCutter;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{

    //Temporary serilizable
    public Transform hitBoxRef;
    private Weapon currentWeapon;
    private Humanoid owner;
    private Collider[] hits;

    private List<SlicingController> slicingControllers = new();

    private int numberOfHits;
    private void Awake()
    {
        hits = new Collider[10];
    }
    private void Start()
    {
        //Moved GetOwner to start to ensure that playerActions actually has the current weapon
        GetOwner();
    }

    public void GetOwner()
    {
        owner = GetComponent<Humanoid>();
        SetCurrentWeapon();
    }

    public void SetCurrentWeapon()
    {
        if (owner is Player player)
        {
            currentWeapon = player.playerActions.currentWeapon;
        }
    }


    public void SetHitBox(Transform parent, Vector3 localCenter, Vector3 size)
    {
        hitBoxRef.parent = parent;
        hitBoxRef.localPosition = localCenter;
        hitBoxRef.localScale = size;
    }

    //Called from animation
    public void OverlapCollider()
    {
        numberOfHits = Physics.OverlapBoxNonAlloc(hitBoxRef.position, hitBoxRef.localScale * 0.5f, hits, hitBoxRef.rotation);

        slicingControllers.Clear();

        for (int i = 0; i < numberOfHits; i++)
        {
            CheckHitInfo(hits[i]);
        }

        SliceRagdoll();

    }
    private void CheckHitInfo(Collider hit)
    {
        if(hit.TryGetComponent(out Health health))
        {
            if(health != owner.health)
            {
                DoDamage(health, hit.ClosestPointOnBounds(currentWeapon.transform.position));
            }
        }
        else if (hit.TryGetComponent(out MeshTarget mesh))
        {
            currentWeapon.Slice(mesh);
        }
        else if(hit.TryGetComponent(out DynamicRagdollPart ragdollPart))
        {
            AddToList(ragdollPart);
        }
    }

    private void DoDamage(Health health, Vector3 hitPoint)
    {
        currentWeapon.Hit(hitPoint);
        health.TakeDamage(currentWeapon);
    }

    private void AddToList(DynamicRagdollPart part)
    {
        SlicingController slicingController = part.GetComponentInParent<SlicingController>();

        if(slicingController != null)
        {
            if (!slicingControllers.Contains(slicingController))
            {
                if(slicingController.animator == null)
                {
                    //This is a bad check, might have to change it later
                    slicingControllers.Add(slicingController);
                }
            }
        }
    }
    private void SliceRagdoll()
    {
        for(int i = 0; i < slicingControllers.Count; i++)
        {
            currentWeapon.Slice(slicingControllers[i].meshTarget);
        }
    }
}
