using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private InputReader inputReader;
    private WeaponSelector weaponSelector;

    private void Awake()
    {
        inputReader = GetComponent<InputReader>();
        weaponSelector = GetComponent<WeaponSelector>();

        inputReader.OnFire += Fire;
        inputReader.OnHeavyFire += HeavyFire;
    }

    private void OnDestroy()
    {
        inputReader.OnFire -= Fire;
        inputReader.OnHeavyFire -= HeavyFire;
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

}
