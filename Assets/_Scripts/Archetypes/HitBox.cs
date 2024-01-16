using Attacks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public event Action<Health, List<ModelContainer>> OnHit;

    [SerializeField] private float center;
    [SerializeField] private Vector3 halfExtends;
    [SerializeField] private ModelContainer[] weapons;
    public Vector3 contactPoint { get; private set; }

    private Archetype archetype;
    private Collider[] hits;
    private int numberOfHits;
    private bool sliceEnded;


    private Attack currentAttack;
    private ActiveWeapon currentWeapon;

    private List<ModelContainer> weaponsToPassOn = new();

    private void Awake()
    {
        hits = new Collider[10];
        archetype = GetComponent<Archetype>();
        archetype.archetypeAnimator.OnAttack += NewAttack;
    }

    private void OnDestroy()
    {
        archetype.archetypeAnimator.OnAttack -= NewAttack;
    }
    private void NewAttack(Attack attack)
    {
        currentAttack = attack;
        currentWeapon = currentAttack.activeWeapon;
    }

    //Called from animation
    public void Anticipation()
    {
        if (!archetype.IsPlayer())
        {
            if(currentWeapon == ActiveWeapon.left)
            {
                EffectManager.instance.Anticipation(weapons[1].Position());
            }
            else
            {
                EffectManager.instance.Anticipation(weapons[0].Position());
            }
            
        }
    }

    //Called from animation
    public void Strike()
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
            //Prevent humanoid from killing themselves
            if(health.owner != archetype.owner)
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
        if(currentWeapon == ActiveWeapon.right)
        {
            weapons[0].CheckSlice(slicable);
        }
        else if(currentWeapon == ActiveWeapon.left)
        {
            weapons[1].CheckSlice(slicable);
        }
        else
        {
            weapons[0].OnSliceDone += DelayedSlice;
            weapons[0].CheckSlice(slicable);  
        }
    }
    private void DelayedSlice(SlicableObject slicable, SlicableObject slicable2)
    {
        weapons[0].OnSliceDone -= DelayedSlice;
        if (!sliceEnded)
        {
            weapons[1].CheckSlice(slicable);
            weapons[1].CheckSlice(slicable2);
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

            Vector3 direction = opponentsWeapon.transform.position - weapons[0].Position();

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
        weaponsToPassOn.Clear();
        if(currentWeapon == ActiveWeapon.right)
        {
            weaponsToPassOn.Add(weapons[0]);
        }
        else if(currentWeapon == ActiveWeapon.left)
        {
            weaponsToPassOn.Add(weapons[1]);
        }
        else
        {
            weaponsToPassOn.Add(weapons[0]);
            weaponsToPassOn.Add(weapons[1]);
        }
        OnHit?.Invoke(health, weaponsToPassOn);
    }

    private bool IsSlicing()
    {
        return weapons[0].IsSlicable();
    }
}
