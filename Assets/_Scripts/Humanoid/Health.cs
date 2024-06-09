using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SlashABit.UI.HudElements;
using RunSettings;
using TMPro;
using EnemyAI;

namespace Stats
{
    public enum ParryType
    {
        None,
        Block,
        Parry,
        PerfectParry
    }
    public class Health : MonoBehaviour
    {


        [SerializeField] protected int startHealth = 100;
        [SerializeField] protected Slider healthSlider;
        [SerializeField] protected CanvasGroup canvasGroupHealth;
        [SerializeField] protected TextMeshProUGUI healthText;

        [SerializeField] protected int startPosture = 100;
        [SerializeField] protected RectTransform[] postureImages;
        [SerializeField] protected CanvasGroup canvasGroupPosture;
        [SerializeField] protected TextMeshProUGUI postureText;

        [Header("Stunned")]
        [SerializeField] private float defaultTimeTillRegen = 2;
        [SerializeField] private float defaultPostureRegen = 10;

        public Humanoid owner;

        private ActiveHudHandler<int> hudHandlerHealth;
        private ActiveHudHandler<float> hudHandlerPosture;


        private Vector2 postureStartSize;

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

            SetPosture(startPosture);

            postureImages[0].DOKill();
            postureImages[1].DOKill();

            postureImages[0].sizeDelta = postureImages[1].sizeDelta = postureStartSize;

            postureRegen = defaultPostureRegen;
            postureText.text = posture.ToString() + "/" + startPosture.ToString();
        }

        public virtual void TakeDamage(int damage, float postureDamage)
        {
            MinusHealth(damage);
            MinusPosture(postureDamage);
            CheckStatus(null);
        }

        public virtual void TakeDamage(Humanoid attacker, Vector3 hitPoint)
        {
            if (IsDead())
            {
                return;
            }

            Weapon attackingWeapon = attacker.currentWeapon;

            MinusHealth(attackingWeapon.currentAttack.damage);
            MinusPosture(attackingWeapon.currentAttack.postureDamage);
           
            attackingWeapon.Hit(hitPoint);


            CheckStatus(attackingWeapon);
        }

        public virtual void MinusHealth(int damage)
        {
            health -= damage;
            SetHealth(health);
        }

        public void MinusPosture(float postureDamage)
        {
            CancelInvoke(nameof(StartRegenPosture));
            canRegenPosture = false;

            
            if(posture - postureDamage > 0)
            {
                SetPosture(posture - postureDamage);
            }
            else
            {
                SetPosture(0);
            }




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
                SetPosture(0);
                DrainedPosture();
            }
            else
            {
                Invoke(nameof(StartRegenPosture), timeTillRegen);
            }
        }


        protected virtual void Dead(Weapon attackingWeapon)
        {
            Dead();
        }

        public virtual void Dead()
        {
            OnDeath?.Invoke();
        }

        protected virtual void DrainedPosture()
        {
            if (!postureDrained)
            {
                postureDrained = true;
                storedHealth = health;
                owner.Stunned();
                Invoke(nameof(StunnedDone), owner.stunnedDuration);
            }
        }
        protected virtual void StunnedDone()
        {
            postureDrained = false;
            StartRegenPosture();
        }

        public bool IsDead()
        {
            if (health <= 0)
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
                    float regen = posture + postureRegen * Time.deltaTime;
                    
                    if( regen > startPosture)
                    {
                        regen = startPosture;
                    }
                    
                    SetPosture(regen);  
                }
                else
                {
                    SetPosture(startPosture);
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
    }
}
