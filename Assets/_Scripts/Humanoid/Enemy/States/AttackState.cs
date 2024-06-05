using UnityEngine;

namespace EnemyAI
{
    public class AttackState : EnemyState
    {
        private bool attacking;
        private bool canStillParry;
        private int currentAttack;
        private int attacksLength;
        private float transition;

        private int numberOfAttacks;
        private int attacksSoFar;
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            StartRotate();

            currentAttack = Random.Range(0, 2) * 2;
            numberOfAttacks = Random.Range(1, 6);
            attacksSoFar = 0;
            transition = 0.25f;
            attacking = false;
            canStillParry = true;

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
            canStillParry = true;
            AttackEnemy attack = currentWeapon.archetype.enemyAttacks[currentAttack];
            enemy.SetAnimation(attack, transition);
            attacksSoFar++;

            transition = attack.transitionDuration;

            enemy.StopMethod();
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
        public override void OverlapCollider()
        {
            canStillParry = false;
        }

        public override void Staggered()
        {
            LeaveStateAndDo(staggeredState, LeaveAttack);
        }
        public override void Hit(Weapon attackingWeapon, Vector3 hitPoint)
        {

            if (enemy.InsideParryFOV() && canStillParry)
            {
                enemy.SetHitPoint(hitPoint);
                LeaveStateAndDo(parryState, LeaveAttack);
            }
            else
            {
                base.Hit(attackingWeapon, hitPoint);
                LeaveStateAndDo(hitState, LeaveAttack);
            }

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
            enemy.SetAnimationWithInt(enemy.attackDoneState);
        }
    }





}