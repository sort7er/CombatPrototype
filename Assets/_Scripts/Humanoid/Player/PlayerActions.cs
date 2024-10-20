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
        if (WeaponAvailable())
        {
            currentArchetype.archetypeAnimator.Fire();
        }
    }
    private void HeavyFire()
    {
        if (WeaponAvailable())
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
        if (WeaponAvailable())
        {
            currentArchetype.archetypeAnimator.Block();
        }
    }

    private void Parry()
    {
        if (WeaponAvailable())
        {
            currentArchetype.archetypeAnimator.Parry();
        }
    }

    private bool WeaponAvailable()
    {
        if(currentArchetype == null)
        {
            return false;
        }
        return !weaponSelector.IsHolstered();
    }
}
