using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private InputReader inputReader;
    private WeaponSelector weaponSelector;
    private TargetAssistance targetAssistance;

    private List<Enemy> enemies = new();

    private Archetype currentArchetype;
    private PlayerData playerData;

    private void Awake()
    {
        inputReader = GetComponent<InputReader>();
        weaponSelector = GetComponent<WeaponSelector>();
        targetAssistance = GetComponent<TargetAssistance>();

        playerData = new PlayerData(GetComponent<PlayerMovement>(), GetComponent<CameraController>(), this, GetComponent<Rigidbody>());

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
    private void NewArchetype(Archetype newArchetype)
    {
        currentArchetype = newArchetype;
    }

    private void Fire()
    {
        if (!weaponSelector.IsHolstered())
        {
            currentArchetype.archetypeAnimator.Fire();
        }
    }
    private void HeavyFire()
    {
        if (!weaponSelector.IsHolstered())
        {
            currentArchetype.archetypeAnimator.HeavyFire();
        }
    }

    private void UniqueFire()
    {
        if (!currentArchetype.archetypeAnimator.isAttacking && !weaponSelector.IsHolstered())
        {
            enemies = targetAssistance.CheckForEnemies();

            if (enemies.Count > 0)
            {
                currentArchetype.uniqueAbility.ExecuteAbility(playerData, enemies[0].transform.position);
            }
            else
            {
                currentArchetype.uniqueAbility.ExecuteAbilityNoTarget(playerData);
            }

            currentArchetype.archetypeAnimator.UniqueFire();


        }

    }
}
