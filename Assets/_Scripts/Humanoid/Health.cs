using System;
using UnityEngine;
using HealthRelated;
using UnityEngine.UI;
using DG.Tweening;

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
    [SerializeField] private Slider healthSlider;


    public event Action OnTakeDamage;
    public event Action OnDeath;
    public int health { get; private set; }

    protected virtual void Awake()
    {
        health = startHealth;
        healthSlider.minValue = 0;
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }

    public virtual void TakeDamage(int damage, DamageType incomingDamage = DamageType.Default)
    {
        if (IsDead())
        {
            return;
        }

        OnTakeDamage?.Invoke();
        health -=  damage;
        healthSlider.DOValue(health, 0.1f).SetEase(Ease.OutFlash);
        if (health <= 0)
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
