using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{

    public event Action OnFire;
    public event Action OnHeavyFire;
    public event Action OnUniqueFire;
    public event Action OnNextWeapon;
    public event Action OnPreviousWeapon;
    public event Action OnHolster;
    public event Action OnBlock;
    public event Action OnParry;


    private bool isHeavy;

    public void Fire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Invoke(nameof(CheckHeavy), 0.02f);
        }
    }
    public void HeavyFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnHeavyFire?.Invoke();
            isHeavy = true;
        }
    }
    public void UniqueFire(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnUniqueFire?.Invoke();
        }
    }

    private void CheckHeavy()
    {
        if(!isHeavy)
        {
            OnFire?.Invoke();
        }
        else
        {
            isHeavy= false;
        }
    }
    public void NextWeapon(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnNextWeapon?.Invoke();
        }
    }
    public void PreviousWeapon(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnPreviousWeapon?.Invoke();
        }
    }
    public void Holster(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnHolster?.Invoke();
        }
    }
    public void BlockAndParry(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnBlock?.Invoke();
        }
        if (ctx.canceled)
        {
            OnParry?.Invoke();
        }
    }
}
