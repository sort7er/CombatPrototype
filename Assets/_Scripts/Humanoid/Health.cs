using System;
using UnityEngine;
using Attacks;
using UnityEngine.UI;
using DG.Tweening;

public class Health : MonoBehaviour
{
    [SerializeField] private int startHealth = 100;
    [SerializeField] private Slider healthSlider;

    [SerializeField] private int startPosture = 100;
    [SerializeField] private Slider postureSlider;
    [SerializeField] private float defaultTimeTillRegen = 4;
    [SerializeField] private float defaultPostureRegen = 10;
    [SerializeField] private float stunnedDuration = 10;

    [SerializeField] private Humanoid owner;


    public event Action OnTakeDamage;
    public event Action OnPostureDrained;
    public event Action OnStaggerDone;
    public event Action OnDeath;
    public int health { get; private set; }
    public float posture { get; private set; }
    private float postureRegen;
    private float timeTillRegen;

    protected int storedHealth;

    private bool canRegen;

    protected virtual void Awake()
    {
        SetUpHealth();
        SetUpPosture();
    }

    private void SetUpHealth()
    {
        health = startHealth;
        healthSlider.minValue = 0;
        healthSlider.maxValue = health;
        healthSlider.value = health;
    }
    private void SetUpPosture()
    {
        posture = startPosture;
        postureSlider.minValue = 0;
        postureSlider.maxValue = startPosture;
        postureSlider.value = posture;
        postureRegen = defaultPostureRegen;
    }

    public virtual void TakeDamage(int damage, int postureDamage = 0)
    {
        WhenLosingHealth(damage, postureDamage);
        CheckHealthStatus(null);
    }

    public virtual void TakeDamage(Weapon attackingWeapon, Vector3 hitPoint)
    {  

        if (IsDead())
        {
            return;
        }

        if(owner is Player)
        {
            CheckForParry(attackingWeapon.owner);
        }


        attackingWeapon.Hit(hitPoint);

        Vector3 direction = transform.position - attackingWeapon.owner.Position();

        owner.AddForce(direction.normalized * attackingWeapon.pushbackForce);

        WhenLosingHealth(attackingWeapon.currentAttack.damage, attackingWeapon.currentAttack.postureDamage);

        CheckHealthStatus(attackingWeapon);
    }

    private void WhenLosingHealth(int damage, int postureDamage = 0)
    {
        OnTakeDamage?.Invoke();
        health -= damage;
        healthSlider.DOValue(health, 0.1f).SetEase(Ease.OutFlash);

        CancelInvoke(nameof(StartRegen));
        canRegen = false;
        posture -= postureDamage;
        postureSlider.DOValue(posture, 0.1f).SetEase(Ease.OutFlash);
        postureRegen = Tools.Remap(health, 0, 100, 1, defaultPostureRegen);
        timeTillRegen = Tools.Remap(health, 0, 100, 10, defaultTimeTillRegen);
    }

    private void CheckHealthStatus(Weapon weapon)
    {
        if (health <= 0)
        {
            health = 0;
            if (weapon != null)
            {
                Dead(weapon);
            }
            else
            {
                Dead();
            }
        }
        else if (posture <= 0)
        {
            DrainedPosture();
        }
        else
        {
            Invoke(nameof(StartRegen), timeTillRegen);
        }
    }

    public void CheckForParry(Humanoid attacker)
    {
        if(owner.parryTimer < attacker.tooLateTime)
        {
            Debug.Log("Too Late");
        }
        else if(owner.parryTimer < attacker.perfectParryTime)
        {
            Debug.Log("Perfect parry");
        }
        else if (owner.parryTimer < attacker.parryTime)
        {
            Debug.Log("Parry");
        }
        else
        {
            Debug.Log("Too early");
        }
    }
    
    public virtual void Dead()
    {
        OnDeath?.Invoke();
    }

    protected virtual void Dead(Weapon attackingWeapon)
    {
        Dead();
    }

    protected virtual void DrainedPosture()
    {
        healthSlider.gameObject.SetActive(false);
        postureSlider.gameObject.SetActive(false);
        storedHealth = health;
        OnPostureDrained?.Invoke();
        Invoke(nameof(StaggerDone), stunnedDuration);

    }
    protected virtual void StaggerDone()
    {
        healthSlider.gameObject.SetActive(true);
        postureSlider.gameObject.SetActive(true);
        health = storedHealth;
        OnStaggerDone?.Invoke();
        StartRegen();
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

    private void StartRegen()
    {
        canRegen = true;
    }
    private void Update()
    {
        if (canRegen)
        {
            if (posture < 100)
            {
                posture += postureRegen * Time.deltaTime;
                postureSlider.value = posture;

            }
            else
            {
                posture = 100;
            }
        }
        
    }
    protected void SetHealth(int newHealth)
    {
        health = newHealth;
    }
}
