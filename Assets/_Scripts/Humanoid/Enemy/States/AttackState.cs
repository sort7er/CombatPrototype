using UnityEngine;

namespace EnemyAI
{
    public class AttackState : EnemyState
    {
        private bool attacking;
        private int currentAttack;
        private int attacksLength;
        private float transition;
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            enemy.DisableMovement();
            enemy.DisableRotation();
            currentAttack = 0;
            transition= 0;

            if (enemy.CheckForWeapon())
            {
                attacksLength = currentWeapon.archetype.enemyAttacks.Length;
            }
        }

        public override void Update()
        {
            base.Update();

            if (PlayerDistance() > enemy.playerDistance + enemy.playerDistanceThreshold && !attacking)
            {
                enemy.SwitchState(enemy.chaseState);
            }
            else
            {
                if (!attacking && enemy.CheckForWeapon())
                {
                    Attack();
                }
            }
        }
        private void Attack()
        {
            attacking = true;
            Attack attack = currentWeapon.archetype.enemyAttacks[currentAttack];
            enemy.SetAnimation(attack, transition);

            transition = attack.transitionDuration;

            enemy.StopFunction();
            enemy.InvokeFunction(Chain, attack.exitTimeSeconds);
            enemy.InvokeFunction(AttackDone, attack.duration);
        }
        private void Chain()
        {
            if(PlayerDistance() <= enemy.playerDistance + enemy.playerDistanceThreshold)
            {
                UpdateCurrentAttack();
                Attack();
            }
        }
        private void AttackDone()
        {
            attacking = false;
            currentAttack = 0;
        }

        private void UpdateCurrentAttack()
        {
            if (currentAttack < attacksLength - 1)
            {
                currentAttack++;
            }
            else
            {
                currentAttack = 0;
            }
        }


        private float PlayerDistance()
        {
            return Vector3.Distance(player.Position(), enemy.Position());
        }


    }





}