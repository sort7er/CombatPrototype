using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{

    public event Action<Vector2> OnMove;
    public event Action OnMoveStarted;
    public event Action OnMoveStopped;
    public event Action OnJump;
    public event Action OnAttack;
    public event Action OnUnique;
    public event Action OnNextWeapon;
    public event Action OnPreviousWeapon;
    public event Action OnBlock;
    public event Action OnParry;


    private bool moveStarted;

    public void Move(InputAction.CallbackContext ctx)
    {
        OnMove?.Invoke(ctx.ReadValue<Vector2>());
        
        if (ctx.performed && !moveStarted)
        {
            moveStarted = true;
            OnMoveStarted?.Invoke();
        }
        else if (ctx.canceled)
        {
            moveStarted = false;
            OnMoveStopped?.Invoke();
        }

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
