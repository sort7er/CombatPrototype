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

    public virtual void TakeDamage(int damage, int postureDamage, HitType incomingDamage = HitType.normal)
    {
        if (IsDead())
        {
            return;
        }

        OnTakeDamage?.Invoke();
        health -=  damage;
        healthSlider.DOValue(health, 0.1f).SetEase(Ease.OutFlash);


        CancelInvoke(nameof(StartRegen));
        canRegen = false;
        posture -= postureDamage;
        postureSlider.DOValue(posture, 0.1f).SetEase(Ease.OutFlash);
        postureRegen = Tools.Remap(health, 0, 100, 1, defaultPostureRegen);
        timeTillRegen = Tools.Remap(health, 0, 100, 10, defaultTimeTillRegen);

        if (health <= 0)
        {
            health = 0;
            Dead(incomingDamage);
        }
        else if(posture <= 0)
        {
            DrainedPosture();
        }
        else
        {
            Invoke(nameof(StartRegen), timeTillRegen);
        }
    }

    protected virtual void Dead(HitType incomingDamage)
    {
        OnDeath?.Invoke();
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
