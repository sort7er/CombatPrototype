using System;
using UnityEngine;

namespace Stats
{

    public class ParryCheck : MonoBehaviour
    {
        public Health health;
        public Humanoid owner;

        private float receivedMultiplier, giveMultiplier;

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

        public void ReturnPostureDamage(Humanoid attacker, Vector3 hitPoint, ParryType parryType, Vector3 direction)
        {
            Vector3 force = direction * attacker.currentWeapon.pushbackForce;

            if (parryType == ParryType.PerfectParry)
            {
                EffectManager.instance.PerfectParry(hitPoint);
                attacker.Staggered();
                owner.PerfectParry();
                DefenceSetup(0.2f, 1.3f, attacker, -force);
            }
            else if (parryType == ParryType.Parry)
            {
                EffectManager.instance.Parry(hitPoint);
                owner.Parry();

                DefenceSetup(0.5f, 1f, attacker, -force);
            }
            else
            {
                EffectManager.instance.Block(hitPoint - direction * 0.1f);

                DefenceSetup(1.5f, 0.25f, owner, force);
            }

            float postureToRemove = CheckRemaingPosture(parryType, attacker.currentWeapon.postureDamage * receivedMultiplier);

            health.TakeDamage(0, postureToRemove);
            attacker.health.TakeDamage(0, owner.currentWeapon.postureDamage * giveMultiplier);
        }
     
        private void DefenceSetup(float recieve, float give, Humanoid owner, Vector3 force)
        {
            receivedMultiplier = recieve;
            giveMultiplier = give;

            owner.AddForce(force);
        }


        private float CheckRemaingPosture(ParryType parryType, float postureToPotentiallyRemove)
        {
            if(parryType == ParryType.Block)
            {
                return postureToPotentiallyRemove;
            }

            if(health.posture > postureToPotentiallyRemove)
            {
                return postureToPotentiallyRemove;
            }
            else
            {
                return Mathf.FloorToInt(health.posture) - 1;
            }
        }
    }
}
