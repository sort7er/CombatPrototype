using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private WeaponSelector weaponSelector;

    public void Fire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!weaponSelector.IsHolstered())
            {
                weaponSelector.CurrentArchetype().Fire();
            }
        }
    }
    public void HeavyFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            if (!weaponSelector.IsHolstered())
            {
                weaponSelector.CurrentArchetype().HeavyFire();
            }
        }
    }
}
