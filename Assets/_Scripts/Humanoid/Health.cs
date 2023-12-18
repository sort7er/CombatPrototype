using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    public event Action OnTakeDamage;
    public event Action OnDeath;
    public int health { get; private set; }

    public virtual void TakeDamage(int damage)
    {
        OnTakeDamage?.Invoke();
        health -=  damage;

        if(health <= 0)
        {
            health = 0;
        }
    }

    protected virtual void Dead()
    {
        OnDeath?.Invoke();
    }

    public bool isDead()
    {
        if(health <= 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

}
