using System;
using System.Collections.Generic;
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

    private List<SlicingObject> slicingObjects = new();

    private void Awake()
    {
        hits = new Collider[10];
        archetype = GetComponent<Archetype>();
    }

    public void Hit(params Transform[] weaponsToCheck)
    {
        slicingObjects.Clear();

        for(int i = 0; i < weaponsToCheck.Length; i++)
        {

            SlicingObject slicingWeapon = weaponsToCheck[i].GetComponent<SlicingObject>(); 

            if (slicingWeapon != null)
            {
                slicingObjects.Add(slicingWeapon);
            }
        }
        CheckHit();
    }

    private void CheckHit()
    {
        numberOfHits = Physics.OverlapBoxNonAlloc(transform.position + transform.forward * center, halfExtends, hits, transform.rotation);

        for(int i = 0; i < numberOfHits; i++)
        {
            //Check if hitting a player / enemy
            if(hits[i].TryGetComponent(out Health health))
            {
                //Check if is blocking or parrying
                ArchetypeAnimator opponentsWeapon = CheckIfAnimator(health);

                if(opponentsWeapon != null)
                {
                    CheckIfBlock(opponentsWeapon, health);
                }
                else
                {
                    DoDamage(health);
                }
            }
            //Check if hitting  a slicible object
            else if (hits[i].TryGetComponent(out SlicableObject sliceble) && slicingObjects.Count > 0)
            {
                for (int j = 0; j < slicingObjects.Count; j++)
                {
                    slicingObjects[j].CheckSlice(sliceble);
                }
            }
        }

    }
    public void DoSlice(SlicableObject slicable)
    {
        //This is called when an enemy dies
        for (int i = 0; i < slicingObjects.Count; i++)
        {
            slicingObjects[i].CheckSlice(slicable);
        }
    }

    public ArchetypeAnimator CheckIfAnimator(Health health)
    {
        if (health.TryGetComponent(out Enemy enemy))
        {
            return enemy.currentArchetype.archetypeAnimator;
        }
        else if (health.TryGetComponent(out WeaponSelector weaponSelector))
        {
            return weaponSelector.currentArchetype.archetypeAnimator;
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
            Debug.Log("No");
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
        Hit(weapons[0]);

        if (!archetype.IsPlayer())
        {
            EffectManager.instance.Anticipation(weapons[0].position);
        }
    }
    public void Lethal2()
    {
        Hit(weapons[1]);

        if (!archetype.IsPlayer())
        {
            EffectManager.instance.Anticipation(weapons[1].position);
        }
    }
    public void Both()
    {
        Hit(weapons);

        if (!archetype.IsPlayer())
        {
            EffectManager.instance.Anticipation(weapons[0].position);
        }
    }
}
