using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SlashABit.UI.HudElements;
using RunSettings;
using TMPro;

public class Health : MonoBehaviour
{
    [SerializeField] private int startHealth = 100;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private CanvasGroup canvasGroupHealth;
    [SerializeField] private TextMeshProUGUI healthText;

    [SerializeField] private int startPosture = 100;
    [SerializeField] private RectTransform[] postureImages;
    [SerializeField] private CanvasGroup canvasGroupPosture;
    [SerializeField] private TextMeshProUGUI postureText;


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

    private bool canRegen;

    protected virtual void Awake()
    {
        hudHandlerHealth = new ActiveHudHandler<int>(3, canvasGroupHealth);
        hudHandlerPosture = new ActiveHudHandler<float>(3, canvasGroupPosture);
        SetUpHealth();
        //SetUpPosture();
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
    //public void SetUpPosture()
    //{
    //    postureStartSize = new Vector2(200, 10);

    //    posture = startPosture;

    //    postureImages[0].DOKill();
    //    postureImages[1].DOKill();

    //    postureImages[0].sizeDelta = postureImages[1].sizeDelta = postureStartSize;

    //    postureRegen = defaultPostureRegen;
    //    postureText.text = posture.ToString() + "/" + startPosture.ToString();
    //}

    public virtual void TakeDamage(int damage, int postureDamage = 0)
    {
        MinusHealth(damage, postureDamage);
        CheckHealthStatus(null);
    }

    public virtual void TakeDamage(Weapon attackingWeapon, Vector3 hitPoint)
    {  

        if (IsDead())
        {
            return;
        }

        attackingWeapon.Hit(hitPoint);



        Vector3 direction = transform.position - attackingWeapon.owner.Position();

        owner.AddForce(direction.normalized * attackingWeapon.pushbackForce);

        MinusHealth(attackingWeapon.currentAttack.damage, attackingWeapon.currentAttack.postureDamage);

        CheckHealthStatus(attackingWeapon);
    }

    


    private void MinusHealth(int damage, int postureDamage = 0)
    {
        OnTakeDamage?.Invoke();
        health -= damage;
        healthSlider.DOValue(health, 0.1f).SetEase(Ease.OutFlash);
        healthText.text = health.ToString() + "/" + startHealth.ToString();

        //CancelInvoke(nameof(StartRegen));
        //canRegen = false;
        //posture -= postureDamage;

        ////postureSlider.DOValue(posture, 0.1f).SetEase(Ease.OutFlash);

        //SetPostureImages(posture);
        //postureRegen = Tools.Remap(health, 0, startHealth, 1, defaultPostureRegen);
        //timeTillRegen = Tools.Remap(health, 0, startHealth, 6, defaultTimeTillRegen);
        //postureText.text = posture.ToString("F0") + "/" + startPosture.ToString("F0");
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
        //else if (posture <= 0)
        //{
        //    DrainedPosture();
        //}
        //else
        //{
        //    Invoke(nameof(StartRegen), timeTillRegen);
        //}
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
        //postureSlider.gameObject.SetActive(false);
        storedHealth = health;
        OnPostureDrained?.Invoke();
        Invoke(nameof(StaggerDone), stunnedDuration);

    }
    protected virtual void StaggerDone()
    {
        healthSlider.gameObject.SetActive(true);
        //postureSlider.gameObject.SetActive(true);
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
        //if (canRegen)
        //{
        //    if (posture < 100)
        //    {
        //        posture += postureRegen * Time.deltaTime;
        //        SetPostureImages(posture);

        //    }
        //    else
        //    {
        //        posture = 100;
        //    }
        //}

        hudHandlerHealth.Update(RunManager.activeHud, health);
        //hudHandlerPosture.Update(RunManager.activeHud, posture);

    }
    protected void SetHealth(int newHealth)
    {
        health = newHealth;
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
}
