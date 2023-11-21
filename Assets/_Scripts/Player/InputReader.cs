using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputReader : MonoBehaviour
{

    public event Action OnFire;
    public event Action OnHeavyFire;


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
