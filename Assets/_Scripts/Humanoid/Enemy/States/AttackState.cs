using UnityEngine;

namespace EnemyAI
{
    public class AttackState : EnemyState
    {
        public bool canParry;

        private bool attacking;
        private int currentAttack;
        private int attacksLength;
        private float transition;

        private int numberOfAttacks;
        private int attacksSoFar;
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            StartRotate();

            currentAttack = 0;// Random.Range(0, 2) * 2;
            numberOfAttacks = Random.Range(1, 6);
            attacksSoFar = 0;
            transition = 0.25f;
            attacking = false;
            canParry = true;

            if (enemy.CheckForWeapon())
            {
                attacksLength = currentWeapon.archetype.enemyAttacks.Length;
            }


        }

        public override void Update()
        {
            base.Update();


            if(!attacking)
            {
                if (!CheckIfCanAttack())
                {
                    LeaveState(standbyState);
                }
                else
                {
                    if (enemy.CheckForWeapon())
                    {
                        Attack();
                    }
                }
            }
        }
        private void Attack()
        {
            StopRotate();
            attacking = true;
            canParry = true;
            AttackEnemy attack = currentWeapon.archetype.enemyAttacks[currentAttack];
            enemy.SetAttack(attack, transition);
            attacksSoFar++;

            transition = attack.transitionDuration;

            enemy.StopMethod();
            enemy.InvokeMethod(CannotParry, enemy.attackParryPeriod);
            enemy.InvokeMethod(StopRotate, 0.25f);
            enemy.InvokeMethod(Chain, attack.exitTimeSeconds);
            enemy.InvokeMethod(AttackDone, attack.duration);
        }
        private void Chain()
        {
            if(CheckIfCanAttack() && attacksSoFar < numberOfAttacks)
            {
                UpdateCurrentAttack();
                Attack();
            }
        }
        private void AttackDone()
        {
            LeaveStateAndDo(standbyState, () => LeaveAttack(0.25f));
        }
        public override void TargetAttacking()
        {
            enemyBehaviour.AttackTargetAttack();
        }
        private void CannotParry()
        {
            canParry = false;
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


        public override void Hit()
        {
            LeaveStateAndDo(hitState, () => LeaveAttack());
        }
        public override void Staggered()
        {
            LeaveStateAndDo(staggeredState, () => LeaveAttack());
        }
        public override void Stunned()
        {
            LeaveStateAndDo(stunnedState,() => LeaveAttack());
        }
        public override void Takedown()
        {
            LeaveStateAndDo(takedownState, () => LeaveAttack());
        }
        private bool CheckIfCanAttack()
        {
            if(enemy.DistanceToTarget() > enemy.minTargetDistance + enemy.playerDistanceThreshold)
            {
                return false;
            }
            if (!enemy.InsideAttackFOV())
            {
                return false;
            }
            return true;
        }
        public void LeaveAttack(float transition = 0)
        {
            enemy.SetAnimationWithInt(enemy.attackDoneState, transition);
        }
    }





}