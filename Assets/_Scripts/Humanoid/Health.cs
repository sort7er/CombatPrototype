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


    [SerializeField] private int startHealth = 100;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private CanvasGroup canvasGroupHealth;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private int startPosture = 100;
    [SerializeField] private RectTransform[] postureImages;
    [SerializeField] private CanvasGroup canvasGroupPosture;
    [SerializeField] private TextMeshProUGUI postureText;

    [Header("Stunned")]
    [SerializeField] private float defaultTimeTillRegen = 2;
    [SerializeField] private float defaultPostureRegen = 10;
    [SerializeField] private float stunnedDuration = 10;

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


        if (IsDead())
        {
            return;
        }

        ParryType parry = CheckForParry(attackingWeapon.owner);

        if(parry == ParryType.None)
        {
            MinusHealth(attackingWeapon.currentAttack.damage);
            MinusPosture(attackingWeapon.currentAttack.postureDamage);
            attackingWeapon.Hit(hitPoint);
        }
        else if(parry == ParryType.PerfectParry)
        {
            MinusPosture(Mathf.FloorToInt(attackingWeapon.currentAttack.postureDamage * 0.2f));
            Debug.Log(attackingWeapon.owner + " should change state to be stunned now");
            EffectManager.instance.PerfectParry(hitPoint);
            attackingWeapon.owner.health.TakeDamage(0, GetOwnersWeapon().currentAttack.postureDamage * 2);
        }
        else
        {
            MinusPosture(Mathf.FloorToInt(attackingWeapon.currentAttack.postureDamage * 0.5f));
            EffectManager.instance.Parry(hitPoint);
            attackingWeapon.owner.health.TakeDamage(0, GetOwnersWeapon().currentAttack.postureDamage);
        }

        Vector3 direction = transform.position - attackingWeapon.owner.Position();
        owner.AddForce(direction.normalized * attackingWeapon.pushbackForce);

        CheckStatus(attackingWeapon);
    }


    public ParryType CheckForParry(Humanoid attacker)
    {
        //Debug.Log("Parry timer: " + owner.parryTimer + ". Too late: " + attacker.tooLateTime + ". Perfect: " + attacker.perfectParryTime + ". Parry: " + attacker.parryTime);

        //if (owner.parryTimer < attacker.tooLateTime)
        //{
        //    Debug.Log("Too Late");
        //    return ParryType.None;
        //}
        if (owner.parryTimer == 0)
        {
            //Debug.Log("No parry");
            return ParryType.None;
        }
        else if (owner.parryTimer < attacker.perfectParryTime)
        {
            //Debug.Log("Perfect parry");
            return ParryType.PerfectParry;
        }
        else if (owner.parryTimer < attacker.parryTime)
        {
            return ParryType.Parry;
        }
        else
        {
            //Debug.Log("Too early");
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


        SetPostureImages(posture);
        postureRegen = Tools.Remap(health, 0, startHealth, 1, defaultPostureRegen);
        timeTillRegen = Tools.Remap(health, 0, startHealth, 6, defaultTimeTillRegen);
        postureText.text = posture.ToString("F0") + "/" + startPosture.ToString("F0");
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
            Invoke(nameof(StaggerDone), stunnedDuration);
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
            if (posture < 100)
            {
                posture += postureRegen * Time.deltaTime;
                SetPostureImages(posture);

            }
            else
            {
                posture = 100;
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

    private void SetPostureImages(float posture)
    {
        float postureValue = Tools.Remap(posture, 0, startPosture, 0, 200);

        postureText.text = posture.ToString("F0") + "/" + startPosture.ToString("F0");

        for (int i = 0; i < postureImages.Length; i++)
        {
            postureImages[i].DOSizeDelta(new Vector2(postureValue, 10), 0.1f).SetEase(Ease.OutFlash);
        }

    }

    private Weapon GetOwnersWeapon()
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
