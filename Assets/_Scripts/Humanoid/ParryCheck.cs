using UnityEngine;

namespace Stats
{

    public class ParryCheck : MonoBehaviour
    {
        public Health health;

        public ParryType CheckForParry(Humanoid defender, Humanoid attacker)
        {
            //Debug.Log("Parry timer: " + defender.parryTimer + ". Perfect: " + attacker.attackPerfectParryWindow + ". Parry: " + attacker.attackParryWindow);

            ParryType noParry;

            if (defender.isBlocking)
            {
                noParry = ParryType.Block;
            }
            else
            {
                noParry = ParryType.None;
            }

            if (defender.parryTimer == 0 || defender.parryTimer > attacker.attackParryWindow)
            {
                return noParry;
            }
            else if (defender.parryTimer <= attacker.attackPerfectParryWindow)
            {
                return ParryType.PerfectParry;
            }
            else
            {
                return ParryType.Parry;
            }
        }

        public void IsDefending(ParryData data)
        {
            float receivedMultiplier;
            float giveMultiplier;

            if (data.parryType == ParryType.PerfectParry)
            {
                receivedMultiplier = 0.2f;
                giveMultiplier = 1.3f;
                EffectManager.instance.PerfectParry(data.hitPoint);
                data.attackingWeapon.owner.Staggered();
                data.defender.PerfectParry();
                data.attackingWeapon.owner.AddForce(-data.direction.normalized * data.attackingWeapon.pushbackForce);
            }
            else if (data.parryType == ParryType.Parry)
            {
                EffectManager.instance.Parry(data.hitPoint);
                receivedMultiplier = 0.5f;
                giveMultiplier = 1f;
                data.defender.Parry();
                data.attackingWeapon.owner.AddForce(-data.direction.normalized * data.attackingWeapon.pushbackForce);
            }
            else
            {
                EffectManager.instance.Block(data.hitPoint - data.direction * 0.1f);
                receivedMultiplier = 1.5f;
                giveMultiplier = 0.25f;
                data.defender.AddForce(data.direction.normalized * data.attackingWeapon.pushbackForce);
            }

            health.MinusPosture(Mathf.FloorToInt(data.postureDamage * receivedMultiplier));

            data.attackingWeapon.owner.health.TakeDamage(0, Mathf.FloorToInt(data.defendingWeapon.postureDamage * giveMultiplier));
        }
    }
}
