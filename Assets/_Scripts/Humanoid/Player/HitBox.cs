using DynamicMeshCutter;
using System.Collections.Generic;
using UnityEngine;
using EnemyAI;
using PlayerSM;

public class HitBox : MonoBehaviour
{

    //Temporary serilizable
    public Transform hitBoxRef;
    public Humanoid owner;
    private Weapon currentWeapon;
    private Attack currentAttack;
    private Collider[] hits;

    private List<SlicingController> slicingControllers = new();

    private int numberOfHits;
    private void Awake()
    {
        hits = new Collider[10];
        owner.OnNewWeapon += SetCurrentWeapon;
        owner.OnAttack += SetCurrentAttack;
    }
    public void SetCurrentWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }
    public void SetCurrentAttack(Attack attack)
    {
        currentAttack = attack;
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
        //Need to check for parry instead of not call this method, to still be able to chain attacks after parry
        if (CheckIfParry())
        {
            return;
        }


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
        if(hit.TryGetComponent(out Humanoid humanoid))
        {
            if(humanoid != owner)
            {
                humanoid.Hit(currentAttack, owner, hit.ClosestPointOnBounds(currentWeapon.weaponPos));
            }
        }
        else if (hit.TryGetComponent(out MeshTarget mesh))
        {
            currentWeapon.Slice(mesh);
        }
    }

    private bool CheckIfParry()
    {
        if (owner is Player player)
        {
            PlayerState state = player.currentState;
            if (state == player.parryState)
            {
                return true;
            }
        }
        if (owner is Enemy enemy)
        {
            EnemyState state = enemy.currentState;
            if (state == enemy.parryState)
            {
                return true;
            }
        }
        return false;
    }
}
