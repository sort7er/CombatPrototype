using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private InputReader inputReader;
    private WeaponSelector weaponSelector;
    private TargetAssistance targetAssistance;
    private PlayerDash playerDash;
    
    private List<Enemy> enemies = new();

    private ArchetypePrefab currentArchetype;

    private void Awake()
    {
        inputReader = GetComponent<InputReader>();
        weaponSelector = GetComponent<WeaponSelector>();
        targetAssistance = GetComponent<TargetAssistance>();
        playerDash = GetComponent<PlayerDash>();

        inputReader.OnFire += Fire;
        inputReader.OnHeavyFire += HeavyFire;
        inputReader.OnUniqueFire += UniqueFire;

        weaponSelector.OnNewArchetype += NewArchetype;
    }

    private void OnDestroy()
    {
        inputReader.OnFire -= Fire;
        inputReader.OnHeavyFire -= HeavyFire;
        inputReader.OnUniqueFire -= UniqueFire;
        weaponSelector.OnNewArchetype -= NewArchetype;
    }
    private void NewArchetype(ArchetypePrefab newArchetype)
    {
        currentArchetype = newArchetype;
    }

    private void Fire()
    {
        if (!weaponSelector.IsHolstered())
        {
            currentArchetype.Fire();
        }
    }
    private void HeavyFire()
    {
        if (!weaponSelector.IsHolstered())
        {
            currentArchetype.HeavyFire();
        }
    }

    private void UniqueFire()
    {
        if (!currentArchetype.isAttacking && !weaponSelector.IsHolstered())
        {
            enemies = targetAssistance.CheckForEnemies();

            if (enemies.Count > 0)
            {
                playerDash.DashForward(enemies[0].transform.position);
            }
            else
            {
                playerDash.DashForward();
            }

            currentArchetype.UniqueFire();
            //currentArchetype.uniqueAbility.ExecuteAbility();

        }

    }
}
