using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SlashABit.UI.HudElements;
using RunSettings;
using TMPro;
using EnemyAI;

public class Health : MonoBehaviour
{
    public enum ParryType
    {
        None,
        Parry,
        PerfectParry
    }


    [SerializeField] protected int startHealth = 100;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private CanvasGroup canvasGroupHealth;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] protected int startPosture = 100;
    [SerializeField] private RectTransform[] postureImages;
    [SerializeField] private CanvasGroup canvasGroupPosture;
    [SerializeField] private TextMeshProUGUI postureText;

    [Header("Stunned")]
    [SerializeField] private float defaultTimeTillRegen = 2;
    [SerializeField] private float defaultPostureRegen = 10;

    public Humanoid owner;

    private ActiveHudHandler<int> hudHandlerHealth;
    private ActiveHudHandler<float> hudHandlerPosture;


    private Vector2 postureStartSize;

    public event Action OnTakeDamage;
    public event Action OnPostureDrained;
    public event Action OnStaggerDone;
    public event Action OnDeath;
    public int health { get; private set; }
    public float posture { get; private set; }
    private float postureRegen;
    private float timeTillRegen;

    protected int storedHealth;

    private bool canRegenPosture;
    private bool postureDrained;

    protected virtual void Awake()
    {
        hudHandlerHealth = new ActiveHudHandler<int>(3, canvasGroupHealth);
        hudHandlerPosture = new ActiveHudHandler<float>(3, canvasGroupPosture);
        SetUpHealth();
        SetUpPosture();
    }

    public void SetUpHealth()
    {
        healthSlider.DOKill();
        health = startHealth;
        healthSlider.minValue = 0;
        healthSlider.maxValue = health;
        healthSlider.value = health;
        healthText.text = health.ToString() + "/" + startHealth.ToString();
    }
    public void SetUpPosture()
    {
        postureStartSize = new Vector2(200, 10);

        posture = startPosture;

        postureImages[0].DOKill();
        postureImages[1].DOKill();

        postureImages[0].sizeDelta = postureImages[1].sizeDelta = postureStartSize;

        postureRegen = defaultPostureRegen;
        postureText.text = posture.ToString() + "/" + startPosture.ToString();
    }

    public virtual void TakeDamage(int damage, int postureDamage)
    {
        MinusHealth(damage);
        MinusPosture(postureDamage);
        CheckStatus(null);
    }

    public virtual void TakeDamage(Weapon attackingWeapon, Vector3 hitPoint)
    {
        //if (IsDead())
        //{
        //    return;
        //}

        ParryType parry = CheckForParry(attackingWeapon.owner, hitPoint);
        Vector3 direction = transform.position - attackingWeapon.owner.Position();
        
        int damage = attackingWeapon.currentAttack.damage;
        int postureDamage = attackingWeapon.currentAttack.postureDamage;


        if (parry == ParryType.None)
        {
            MinusHealth(damage);
            MinusPosture(postureDamage);
            attackingWeapon.Hit(hitPoint);
            owner.AddForce(direction.normalized * attackingWeapon.pushbackForce);

        }
        else
        {
            float receivedMultiplier;
            float giveMultiplier;

            if(parry == ParryType.PerfectParry)
            {
                receivedMultiplier = 0.2f;
                giveMultiplier = 1.3f;
                EffectManager.instance.PerfectParry(hitPoint);
                Tools.GetEnemy(attackingWeapon.owner).Staggered();
                Tools.GetPlayer(owner).playerActions.PerfectParry();
            }
            else
            {
                EffectManager.instance.Parry(hitPoint);
                receivedMultiplier = 0.5f;
                giveMultiplier = 1f;
                Tools.GetPlayer(owner).playerActions.Parry();
            }

            MinusPosture(Mathf.FloorToInt(postureDamage * receivedMultiplier));

            attackingWeapon.owner.health.TakeDamage(0, Mathf.FloorToInt(GetOwnersWeapon().postureDamage * giveMultiplier));
            attackingWeapon.owner.AddForce(-direction.normalized * attackingWeapon.pushbackForce);
        }

        CheckStatus(attackingWeapon);
    }


    public ParryType CheckForParry(Humanoid attacker, Vector3 hitPoint)
    {
        //Debug.Log("Parry timer: " + owner.parryTimer + ". Too late: " + attacker.tooLateTime + ". Perfect: " + attacker.perfectParryTime + ". Parry: " + attacker.parryTime);


        if (owner.parryTimer == 0)
        {
            return ParryType.None;
        }
        else if (owner.parryTimer < attacker.attackPerfectParryWindow)
        {
            return ParryType.PerfectParry;
        }
        else if (owner.parryTimer < attacker.attackParryWindow)
        {
            return ParryType.Parry;
        }
        else
        {
            EffectManager.instance.ParryFeedback(hitPoint, "TOO EARLY!");
            return ParryType.None;
        }
    }

    private void MinusHealth(int damage)
    {
        OnTakeDamage?.Invoke();
        health -= damage;
        SetHealth(health);
    }

    private void MinusPosture(int postureDamage)
    {
        CancelInvoke(nameof(StartRegenPosture));
        canRegenPosture = false;

        posture -= postureDamage;
        SetPosture(posture);

        postureRegen = Tools.Remap(health, 0, startPosture, 1, defaultPostureRegen);
        timeTillRegen = Tools.Remap(health, 0, startHealth, 6, defaultTimeTillRegen);
    }

    private void CheckStatus(Weapon weapon)
    {
        if (health <= 0)
        {
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
            posture= 0;
            DrainedPosture();
        }
        else
        {
            Invoke(nameof(StartRegenPosture), timeTillRegen);
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
        if (!postureDrained)
        {
            postureDrained = true;
            healthSlider.gameObject.SetActive(false);
            canvasGroupPosture.gameObject.SetActive(false);
            storedHealth = health;
            OnPostureDrained?.Invoke();

            float duration = 4;

            if(owner is Enemy enemy)
            {
                duration = Tools.GetEnemy(owner).stunnedDuration;
            }

            Invoke(nameof(StaggerDone), duration);
        }
    }
    protected virtual void StaggerDone()
    {
        postureDrained = false;
        healthSlider.gameObject.SetActive(true);
        canvasGroupPosture.gameObject.SetActive(true);
        OnStaggerDone?.Invoke();
        StartRegenPosture();
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

    private void StartRegenPosture()
    {
        canRegenPosture = true;
    }
    private void Update()
    {
        if (canRegenPosture)
        {
            if (posture < startPosture)
            {
                posture += postureRegen * Time.deltaTime;
                SetPostureImages(posture);

            }
            else
            {
                posture = startPosture;
            }
        }

        hudHandlerHealth.Update(RunManager.activeHud, health);
        hudHandlerPosture.Update(RunManager.activeHud, posture);

    }
    protected void SetHealth(int newHealth)
    {
        health = newHealth;
        healthSlider.DOValue(health, 0.1f).SetEase(Ease.OutFlash);
        healthText.text = health.ToString() + "/" + startHealth.ToString();
    }
    protected void SetPosture(float newPosture)
    {
        posture = newPosture;
        SetPostureImages(posture);
        postureText.text = posture.ToString("F0") + "/" + startPosture.ToString("F0");
    }

    private void SetPostureImages(float posture)
    {
        float postureValue = Tools.Remap(posture, 0, startPosture, 0, 200);

        postureText.text = posture.ToString("F0") + "/" + startPosture.ToString("F0");

        for (int i = 0; i < postureImages.Length; i++)
        {
            postureImages[i].DOSizeDelta(new Vector2(postureValue, 10), 0.1f).SetEase(Ease.OutFlash);
        }

    }

    protected Enemy GetEnemy()
    {
        if(owner is Enemy enemy)
        {
            return enemy;
        }
        else return null;
    }

    protected Player GetPlayer()
    {
        if (owner is Player player)
        {
            return player;
        }
        else return null;
    }

    protected Weapon GetOwnersWeapon()
    {
        if(owner is Player player)
        {
            return player.playerActions.currentWeapon;
        }
        else if(owner is Enemy enemy)
        {
            return enemy.currentWeapon;
        }
        else
        {
            return null;
        }
    }
}
