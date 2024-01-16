using HumanoidTypes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Archetype : MonoBehaviour
{
    public string archetypeName;
    public TargetAssistanceParams targetAssistanceParams;


    public ArchetypeAnimator archetypeAnimator { get; private set; }
    public UniqueAbility uniqueAbility { get; private set; }

    public Humanoid owner { get; private set; }
    public HitBox hitBox { get; private set; }
    public WeaponContainer weaponContainer { get; private set; }

    public SlicingObject[] slicingObjects;

    private void Awake()
    {
        FindReferences();
        hitBox.OnHit += OnHit;
    }

    private void FindReferences()
    {
        archetypeAnimator = GetComponent<ArchetypeAnimator>();
        uniqueAbility = GetComponent<UniqueAbility>();
        hitBox = GetComponent<HitBox>();
        FindOwner();
    }

    private void FindOwner()
    {
        if (transform.parent.TryGetComponent(out WeaponContainer container))
        {
            weaponContainer = container;
            weaponContainer.OnOwnerFound += FoundOwner;
            weaponContainer.FindOwner();
        }
    }
    private void FoundOwner(Humanoid newOwner)
    {
        weaponContainer.OnOwnerFound -= FoundOwner;
        owner = newOwner;
        for(int i = 0; i < slicingObjects.Length; i++)
        {
            slicingObjects[i].OwnerFound(newOwner);
        }
    }

    private void Start()
    {
        //Temporary until a pickupfunction works
        if (transform.parent.TryGetComponent(out WeaponContainer container))
        {
            owner = container.owner;
        }
    }
    private void OnDestroy()
    {
        hitBox.OnHit -= OnHit;
    }
    private void OnHit(Health health)
    {
        health.TakeDamage(archetypeAnimator.currentAttack.damage, this, archetypeAnimator.currentAttack.damageType);
        //EffectManager.instance.Hit(hitBox.contactPoint, weaponTrigger.swingDir, weaponTrigger.upDir);
    }

    public void UniqueAttack(List<Enemy> enemies, PlayerData playerData)
    {
        if (enemies.Count > 0)
        {
            uniqueAbility.ExecuteAbility(playerData, targetAssistanceParams, enemies);
        }
        else
        {
            uniqueAbility.ExecuteAbilityNoTarget(playerData);
        }

        archetypeAnimator.UniqueFire();
    }
    
    public bool IsPlayer()
    {
        if (owner.ownerType == OwnerType.Player)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
