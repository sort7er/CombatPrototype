using HumanoidTypes;
using System.Collections.Generic;
using UnityEngine;
using Attacks;
using HealthRelated;

public class Archetype : MonoBehaviour
{
    public string archetypeName;
    public TargetAssistanceParams targetAssistanceParams;


    public ArchetypeAnimator archetypeAnimator;
    public UniqueAbility uniqueAbility;
    public HitBox hitBox;

    public Humanoid owner { get; private set; }
    public WeaponContainer weaponContainer { get; private set; }

    public SlicingObject[] slicingObjects;

    private void Awake()
    {
        FindOwner();
        hitBox.OnHit += OnHit;
        hitBox.OnPostureOnly += OnlyPosture;
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
        hitBox.OnPostureOnly -= OnlyPosture;
    }
    private void OnHit(Attack attack, Health health, List<ModelContainer> weapons)
    {
        health.TakeDamage(attack.damage, attack.postureDamage, this, attack.damageType);

        if(attack.attributeAffected == AttributeAffected.normal)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                EffectManager.instance.Hit(weapons[i].Position(), weapons[i].Direction(), weapons[i].UpDir());
            }
        }
    }
    private void OnlyPosture(int postureDamage, Health health)
    {
        health.TakeDamage(0, postureDamage, this, DamageType.Default);
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
