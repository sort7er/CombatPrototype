using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{

    public event Action<Vector2> OnMove;
    public event Action OnJump;
    public event Action OnAttack;
    public event Action OnUnique;
    public event Action OnNextWeapon;
    public event Action OnPreviousWeapon;
    public event Action OnBlock;
    public event Action OnParry;


    public void Move(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(ctx.ReadValue<Vector2>());
    }

    public void Jump(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnJump?.Invoke();
        }
    }

    public void Attack(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnAttack?.Invoke();
        }
    }
    public void Unique(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            OnUnique?.Invoke();
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
