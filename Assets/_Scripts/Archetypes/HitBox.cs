using System;
using UnityEngine;

public class HitBox : MonoBehaviour
{
    public event Action<Health> OnHit;

    [SerializeField] private float center;
    [SerializeField] private Vector3 halfExtends;
    public Vector3 contactPoint { get; private set; }

    private Archetype archetype;
    private Collider[] hits;
    private int numberOfHits;

    private Transform currentWeapon;


    private void Awake()
    {
        archetype = GetComponent<Archetype>();
        hits = new Collider[10];
    }

    public void CheckHit()
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

            Vector3 direction = opponentsWeapon.transform.position - currentWeapon.position;

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
}
