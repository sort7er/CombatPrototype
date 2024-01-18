using Attacks;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HitBox : MonoBehaviour
{

    public event Action<Attack, Health, List<ModelContainer>> OnHit;
    public event Action<int, Health> OnPostureOnly;
    public event Action<bool> OnCanBeParried;

    [SerializeField] private float center;
    [SerializeField] private Vector3 halfExtends;
    [SerializeField] private ModelContainer[] weapons;
    [SerializeField] private ParticleSystem[] trail;
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
        archetype.archetypeAnimator.OnSwingDone += DisableTrail;
    }

    private void OnDestroy()
    {
        archetype.archetypeAnimator.OnAttack -= NewAttack;
        archetype.archetypeAnimator.OnSwingDone -= DisableTrail;
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
        EnableTrail(currentAttack.activeWeapon);
        OnCanBeParried?.Invoke(true);
    }

    //Called from animation
    public void Strike()
    {
        numberOfHits = Physics.OverlapBoxNonAlloc(transform.position + transform.forward * center, halfExtends, hits, transform.rotation);
        OnCanBeParried?.Invoke(false);

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
                //Check if current attack is a parry
                if (currentAttack.attributeAffected == AttributeAffected.normal)
                {
                    CheckIfBlock(health);
                }
                else
                {
                    CheckIfCanParry(health);
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

    private void CheckIfCanParry(Health health)
    {
        ArchetypeAnimator opponentsWeapon = CheckIfAnimator(health);
        if (opponentsWeapon != null)
        {
            if (opponentsWeapon.canBeParried)
            {
                OnlyPosture(currentAttack.postureDamage, health);
                Vector3 direction = opponentsWeapon.transform.position - weapons[0].Position();

                EffectManager.instance.Parry(transform.position + direction * 0.5f + Vector3.up * 0.1f);
            }
        }
    }
    //Check if is blocking
    private void CheckIfBlock(Health health)
    {
        ArchetypeAnimator opponentsWeapon = CheckIfAnimator(health);
        if (opponentsWeapon != null)
        {
            if (opponentsWeapon.isBlocking)
            {
                OnlyPosture(currentAttack.damage * 3, health);
            }
            else
            {
                DoDamage(health);
            }
        }
        else
        {
            DoDamage(health);
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

        OnHit?.Invoke(currentAttack, health, weaponsToPassOn);
    }

    private void OnlyPosture(int postureDamage, Health health)
    {
        OnPostureOnly?.Invoke(postureDamage, health);
    }

    private bool IsSlicing()
    {
        return weapons[0].IsSlicable();
    }
    private void EnableTrail(ActiveWeapon activeWeapon)
    {
        if (trail.Length > 0)
        {
            if (activeWeapon == ActiveWeapon.right)
            {
                trail[0].Play();
            }
            else if (activeWeapon == ActiveWeapon.left)
            {
                trail[1].Play();
            }
            else
            {
                trail[0].Play();
                trail[1].Play();

            }
        }
    }
    private void DisableTrail()
    {
        if (trail.Length > 0)
        {
            trail[0].Stop();
            if (trail.Length > 1)
            {
                trail[1].Stop();
            }
        }
    }
}
