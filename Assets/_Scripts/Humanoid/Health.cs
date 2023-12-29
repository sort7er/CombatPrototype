using System;
using UnityEngine;
using HealthRelated;


namespace HealthRelated
{
    public enum DamageType
    {
        Default,
        Slice,
        Crubmle
    }
}

public class Health : MonoBehaviour
{
    [SerializeField] private int startHealth = 100;



    public event Action OnTakeDamage;
    public event Action OnDeath;
    public int health { get; private set; }

    protected virtual void Awake()
    {
        health = startHealth;
    }

    public virtual void TakeDamage(int damage, DamageType incomingDamage = DamageType.Default)
    {
        if (IsDead())
        {
            return;
        }

        OnTakeDamage?.Invoke();
        health -=  damage;

        if(health <= 0)
        {
            health = 0;
            Dead(incomingDamage);
        }
    }

    protected virtual void Dead(DamageType incomingDamage)
    {
        OnDeath?.Invoke();
    }

    public bool IsDead()
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
