using UnityEngine;

namespace EnemyAI
{
    public class AttackState : EnemyState
    {
        private bool attacking;
        private bool rotateTowardsPlayer;
        private int currentAttack;
        private int attacksLength;
        private float transition;

        private int numberOfAttacks;
        private int attacksSoFar;
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            currentAttack = Random.Range(0, 2) * 2;
            numberOfAttacks = Random.Range(1, 6);
            attacksSoFar = 0;
            transition = 0.25f;
            attacking = false;
            rotateTowardsPlayer = true;

            if (enemy.CheckForWeapon())
            {
                attacksLength = currentWeapon.archetype.enemyAttacks.Length;
            }


        }

        public override void Update()
        {
            base.Update();
            if (rotateTowardsPlayer)
            {
                enemy.RotateToTarget(player.Position(), player.Position());
            }



            if (!CheckIfCanAttack() && !attacking)
            {
                LeaveState(standbyState);
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
            rotateTowardsPlayer = true;
            AttackEnemy attack = currentWeapon.archetype.enemyAttacks[currentAttack];
            enemy.SetAnimation(attack, transition);
            attacksSoFar++;

            transition = attack.transitionDuration;

            enemy.StopFunction();
            enemy.InvokeFunction(StopRotate, 0.25f);
            enemy.InvokeFunction(Chain, attack.exitTimeSeconds);
            enemy.InvokeFunction(AttackDone, attack.duration);
        }
        private void StopRotate()
        {
            rotateTowardsPlayer = false;
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
            LeaveState(standbyState);
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

        public override void Staggered()
        {
            LeaveStateAndDo(staggeredState, LeaveAttack);
        }
        public override void Hit()
        {
            
            LeaveStateAndDo(hitState, LeaveAttack);
        }

        public override void Stunned()
        {
            LeaveStateAndDo(stunnedState, LeaveAttack);
        }
        private bool CheckIfCanAttack()
        {
            if(enemy.DistanceToTarget() > enemy.minPlayerDistance + enemy.playerDistanceThreshold)
            {
                return false;
            }
            if (!enemy.InsideAttackFOV())
            {
                return false;
            }
            return true;
        }
        private void LeaveAttack()
        {
            enemyAnimator.animator.CrossFadeInFixedTime(enemy.attackDoneState, 0);
        }
    }





}