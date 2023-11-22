using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{

    public event Action OnFire;
    public event Action OnHeavyFire;
    public event Action OnUniqueFire;


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

}
