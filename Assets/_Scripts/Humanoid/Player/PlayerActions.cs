using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    public Archetype currentArchetype { get; private set; }
    [SerializeField] private float parryWindow = 0.1f;

    private InputReader inputReader;
    private WeaponSelector weaponSelector;
    private TargetAssistance targetAssistance;

    private List<Enemy> enemies = new();

    private PlayerData playerData;

    private bool parrying;
    private bool blocking;

    private void Awake()
    {
        inputReader = GetComponent<InputReader>();
        weaponSelector = GetComponent<WeaponSelector>();
        targetAssistance = GetComponent<TargetAssistance>();

        playerData = new PlayerData(GetComponent<PlayerMovement>(), GetComponent<CameraController>(), this, GetComponent<Rigidbody>());

        inputReader.OnFire += Fire;
        inputReader.OnHeavyFire += HeavyFire;
        inputReader.OnUniqueFire += UniqueFire;
        inputReader.OnBlock += Block;
        inputReader.OnParry += Parry;

        weaponSelector.OnNewArchetype += NewArchetype;
    }

    private void OnDestroy()
    {
        inputReader.OnFire -= Fire;
        inputReader.OnHeavyFire -= HeavyFire;
        inputReader.OnUniqueFire -= UniqueFire;
        weaponSelector.OnNewArchetype -= NewArchetype;
        inputReader.OnBlock -= Block;
        inputReader.OnParry -= Parry;
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
        if (WeaponAvailable())
        {
            enemies = targetAssistance.CheckForEnemies(currentArchetype.targetAssistanceParams, out int numIdealTargets);

            currentArchetype.UniqueAttack(enemies, playerData);
        }

    }
    private void Block()
    {
        if(WeaponAvailable())
        {
            parrying = false;
            blocking = false;
            currentArchetype.archetypeAnimator.Block();
            Invoke(nameof(CheckForParry), parryWindow);
        }
    }

    private void CheckForParry()
    {
        if (parrying)
        {
            currentArchetype.archetypeAnimator.Parry();
        }
        else
        {
            blocking = true;
        }
    }

    private void Parry()
    {
        if (!blocking)
        {
            parrying = true;
        }
        else
        {
            currentArchetype.archetypeAnimator.ActionDone();
        }
    }

    private bool WeaponAvailable()
    {
        return !currentArchetype.archetypeAnimator.isAttacking && !weaponSelector.IsHolstered();
    }
}
