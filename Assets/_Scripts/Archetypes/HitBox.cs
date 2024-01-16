using System;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public event Action<Health> OnHit;

    [SerializeField] private float center;
    [SerializeField] private Vector3 halfExtends;
    [SerializeField] public Transform[] weapons;
    public Vector3 contactPoint { get; private set; }

    private Archetype archetype;
    private Collider[] hits;
    private int numberOfHits;
    private bool sliceEnded;

    private SlicingObject[] slicingWeapons;

    public enum HittingWeapon
    {
        right,
        left,
        both,
    }

    private HittingWeapon currentHittingWeapon;

    private void Awake()
    {
        hits = new Collider[10];
        archetype = GetComponent<Archetype>();
        DoWeSlice();
    }

    private void DoWeSlice()
    {
        //Check if there is slicing on the weapons
        slicingWeapons = new SlicingObject[weapons.Length];
        for(int i = 0; i < weapons.Length; i++)
        {
            if (weapons[i].TryGetComponent(out SlicingObject slice))
            {
                slicingWeapons[i] = slice;
            }
        }
    }

    private void CheckHit()
    {
        numberOfHits = Physics.OverlapBoxNonAlloc(transform.position + transform.forward * center, halfExtends, hits, transform.rotation);

        for(int i = 0; i < numberOfHits; i++)
        {
            CheckHitInfo(hits[i]);
        }

    }
    private void CheckHitInfo(Collider hit)
    {
        //Check if hitting a player / enemy
        if (hit.TryGetComponent(out Health health))
        {
            //Check if is blocking or parrying
            ArchetypeAnimator opponentsWeapon = CheckIfAnimator(health);

            if (opponentsWeapon != null)
            {
                CheckIfBlock(opponentsWeapon, health);
            }
            else
            {
                DoDamage(health);
            }
        }
        //Check if hitting  a slicible object
        else if (IsSlicing())
        {
            if (hit.TryGetComponent(out SlicableObject sliceble))
            {
                Slice(sliceble);
            }
        }
    }

    public void Slice(SlicableObject slicable)
    {
        sliceEnded = false;
        if(currentHittingWeapon == HittingWeapon.right)
        {
            slicingWeapons[0].CheckSlice(slicable);
        }
        else if( currentHittingWeapon == HittingWeapon.left)
        {
            slicingWeapons[1].CheckSlice(slicable);
        }
        else
        {
            slicingWeapons[0].OnSliceDone += DelayedSlice;
            slicingWeapons[0].CheckSlice(slicable);  
        }
    }
    private void DelayedSlice(SlicableObject slicable, SlicableObject slicable2)
    {
        slicingWeapons[0].OnSliceDone -= DelayedSlice;
        if (!sliceEnded)
        {
            slicingWeapons[1].CheckSlice(slicable);
            slicingWeapons[1].CheckSlice(slicable2);
            sliceEnded = true;
        }
    }

    public ArchetypeAnimator CheckIfAnimator(Health health)
    {
        if (health.TryGetComponent(out Enemy enemy))
        {
            if(enemy.currentArchetype != null)
            {
                return enemy.currentArchetype.archetypeAnimator;
            }
            else
            {
                return null;
            }
        }
        else if (health.TryGetComponent(out WeaponSelector weaponSelector))
        {
            if (weaponSelector.currentArchetype != null)
            {
                return weaponSelector.currentArchetype.archetypeAnimator;
            }
            else
            {
                return null;
            }
        }
        else
        {
            return null;
        }
    }
    private void CheckIfBlock(ArchetypeAnimator opponentsWeapon, Health health)
    {
        if (opponentsWeapon.isParrying)
        {
            archetype.owner.Staggered();

            Vector3 direction = opponentsWeapon.transform.position - weapons[0].position;

            EffectManager.instance.Parry(transform.position + direction * 0.5f + Vector3.up * 0.3f);
        }
        else if (opponentsWeapon.isBlocking)
        {
            Debug.Log("Block");
        }
        else
        {
            //Debug.Log("No");
            DoDamage(health);
        }
    }
    private void DoDamage(Health health)
    {

        //Get upDirection and contactpoint from currentWeapon

        OnHit?.Invoke(health);
    }

    //Called from the animations
    public void Lethal()
    {
        currentHittingWeapon = HittingWeapon.right;
        CheckHit();

        if (!archetype.IsPlayer())
        {
            EffectManager.instance.Anticipation(weapons[0].position);
        }
    }
    public void Lethal2()
    {
        currentHittingWeapon = HittingWeapon.left;
        CheckHit();

        if (!archetype.IsPlayer())
        {
            EffectManager.instance.Anticipation(weapons[1].position);
        }
    }
    public void Both()
    {
        currentHittingWeapon = HittingWeapon.both;
        CheckHit();

        if (!archetype.IsPlayer())
        {
            EffectManager.instance.Anticipation(weapons[0].position);
        }
    }
    private bool IsSlicing()
    {
        if (slicingWeapons[0] != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
