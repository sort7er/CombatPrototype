using System.Collections.Generic;
using UnityEngine;

public class Archetype : MonoBehaviour
{


    public string archetypeName;
    public ArchetypeAnimator archetypeAnimator;
    public UniqueAbility uniqueAbility;
    public WeaponTrigger[] weaponTrigger;
    public TargetAssistanceParams targetAssistanceParams;

    private List<Health> hits = new();

    private void Awake()
    {
        for(int i = 0; i < weaponTrigger.Length; i++)
        {
            weaponTrigger[i].OnHit += OnHit;
        }
        archetypeAnimator.OnLethal += Lethal;
        archetypeAnimator.OnLethal2 += Lethal2;
        archetypeAnimator.OnNotLethal += NotLethal;
    }
    private void OnDestroy()
    {
        for (int i = 0; i < weaponTrigger.Length; i++)
        {
            weaponTrigger[i].OnHit -= OnHit;
        }
        archetypeAnimator.OnLethal -= Lethal;
        archetypeAnimator.OnLethal2 -= Lethal2;
        archetypeAnimator.OnNotLethal -= NotLethal;
    }
    private void OnHit(Health health, WeaponTrigger weaponTrigger)
    {
        if(!hits.Contains(health))
        {
            hits.Add(health);
            health.TakeDamage(50);
            EffectManager.instance.Hit(weaponTrigger.contactPoint, weaponTrigger.swingDir, weaponTrigger.upDir);
        }
    }
    private void Lethal()
    {
        hits.Clear();
        weaponTrigger[0].EnableCollider();
    }
    private void Lethal2()
    {
        hits.Clear();
        weaponTrigger[1].EnableCollider();
    }
    private void NotLethal()
    {
        for (int i = 0; i < weaponTrigger.Length; i++)
        {
            weaponTrigger[i].DisableCollider();
        }
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

}
