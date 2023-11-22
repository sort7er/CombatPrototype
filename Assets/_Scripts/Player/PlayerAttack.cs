using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private InputReader inputReader;
    private WeaponSelector weaponSelector;
    private TargetAssistance targetAssistance;
    
    private List<Enemy> enemies = new();

    private void Awake()
    {
        inputReader = GetComponent<InputReader>();
        weaponSelector = GetComponent<WeaponSelector>();
        targetAssistance = GetComponent<TargetAssistance>();

        inputReader.OnFire += Fire;
        inputReader.OnHeavyFire += HeavyFire;
        inputReader.OnUniqueFire += UniqueFire;
    }

    private void OnDestroy()
    {
        inputReader.OnFire -= Fire;
        inputReader.OnHeavyFire -= HeavyFire;
        inputReader.OnUniqueFire -= UniqueFire;
    }

    private void Fire()
    {
        if (!weaponSelector.IsHolstered())
        {
            weaponSelector.CurrentArchetype().Fire();
        }
    }
    private void HeavyFire()
    {
        if (!weaponSelector.IsHolstered())
        {
            weaponSelector.CurrentArchetype().HeavyFire();
        }
    }

    private void UniqueFire()
    {

        TargetAssistanceTest();


        if (!weaponSelector.IsHolstered())
        {
            weaponSelector.CurrentArchetype().UniqueFire();
        }
    }

    private void TargetAssistanceTest()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            enemies[i].SetDefault();
        }

        enemies = targetAssistance.CheckForEnemies();

        if (enemies.Count > 1)
        {
            enemies[1].SetAsSecondTarget();
            enemies[0].SetAsTarget();
        }
        else if (enemies.Count > 0)
        {
            enemies[0].SetAsTarget();
        }
    }

}
