using System.Collections;
using UnityEngine;

namespace EnemyStates
{
    public class AttackState : EnemyState
    {
        private bool coolDown;
        private Enemy enemy;
        public override void EnterState(Enemy enemy)
        {
            this.enemy = enemy;
            enemy.DisableMovement();
            enemy.InvokeFunction(CooldownDone, Random.Range(enemy.attackCooldown, enemy.attackCooldown + 1));
        }

        public override void UpdateState(Enemy enemy)
        {
            enemy.LookAtTarget(enemy.player.Position());

            if (HasArchetype(enemy))
            {
                if (Vector3.Distance(enemy.player.Position(), enemy.Position()) > enemy.playerDistance && !IsActive(enemy))
                {
                    enemy.SwitchState(enemy.chaseState);
                }

                //Attack if there is a weapon, and if there is no longer any attackcooldown, should have a propar check to see if holding a weapon
                if (!IsActive(enemy) && !coolDown)
                {
                    SelectCombo(enemy);
                    coolDown = true;
                }
            }
            else
            {
                //If no archetype, go look for weapon or something. For now just chase player
                if (Vector3.Distance(enemy.player.Position(), enemy.Position()) > enemy.playerDistance)
                {
                    enemy.SwitchState(enemy.chaseState);
                }
            }
        }
        public override void Staggered(Enemy enemy)
        {
            enemy.StopFunction();
            enemy.SwitchState(enemy.staggeredState);
        }

        public void SelectCombo(Enemy enemy)
        {
            int numberOfAttacks = Random.Range(1, 4);
            for (int i = 0; i < 3; i++)
            {
                enemy.InvokeCoroutine(RandomFire(enemy, i * 0.01f));
            }
        }


        private IEnumerator RandomFire(Enemy enemy, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            int rnd = Random.Range(0, 2);

            if (rnd == 0)
            {
                enemy.currentArchetype.archetypeAnimator.Fire();
            }
            else
            {
                enemy.currentArchetype.archetypeAnimator.HeavyFire();
            }
        }
        public void AttackDone()
        {
            enemy.InvokeFunction(CooldownDone, Random.Range(enemy.attackCooldown, enemy.attackCooldown + 1));
        }
        public void CooldownDone()
        {
            coolDown = false;
        }

        private bool HasArchetype(Enemy enemy)
        {
            if (enemy.currentArchetype != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool IsActive(Enemy enemy)
        {
            if(enemy.currentArchetype != null && enemy.currentArchetype.gameObject.activeSelf)
            {
                return enemy.currentArchetype.archetypeAnimator.isAttacking;
            }
            else
            {
                return true;
            }
        }
    }
}

