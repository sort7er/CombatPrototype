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

    private bool cannotParry;

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
        if (!currentArchetype.archetypeAnimator.IsActive() && !weaponSelector.IsHolstered())
        {
            enemies = targetAssistance.CheckForEnemies(currentArchetype.targetAssistanceParams, out int numIdealTargets);

            currentArchetype.UniqueAttack(enemies, playerData);
        }

    }
    private void Block()
    {
        if(!currentArchetype.archetypeAnimator.IsActive() && !weaponSelector.IsHolstered())
        {
            cannotParry = false;
            currentArchetype.archetypeAnimator.Block();
            Invoke(nameof(CannotParry), parryWindow);
            
        }
    }
    private void CannotParry()
    {
        cannotParry = true;
    }
    private void Parry()
    {
        if (currentArchetype.archetypeAnimator.isDefending && !weaponSelector.IsHolstered())
        {
            if (!cannotParry)
            {
                CancelInvoke(nameof(CannotParry));
                currentArchetype.archetypeAnimator.Parry();
            }
            else
            {
                currentArchetype.archetypeAnimator.ActionDone();
            }
        }
    }
}
