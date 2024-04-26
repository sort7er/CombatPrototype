using DynamicMeshCutter;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;

public class HitBox : MonoBehaviour
{

    //Temporary serilizable
    public Transform hitBoxRef;
    private Weapon currentWeapon;
    private Humanoid owner;
    private  Collider[] hits;

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
        if(owner is Enemy enemy)
        {
            currentWeapon = enemy.currentWeapon;
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

        for (int i = 0; i < hits.Length; i++)
        {
            hits[i] = null;
        }

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
    }

    private void DoDamage(Health health, Vector3 hitPoint)
    {
        if (!health.IsDead() && GameTracking.instance != null)
        {
            DoGameTracking(health);
        }
        
        health.TakeDamage(currentWeapon, hitPoint);
    }

    private void DoGameTracking(Health health)
    {
        if (owner is Player)
        {
            GameTracking.instance.AddDamageDealt(currentWeapon.currentAttack.damage);
        }
        if (health.owner is Player)
        {
            GameTracking.instance.AddDamageReceived(currentWeapon.currentAttack.damage);
        }
    }
}
