using System;
using UnityEngine.AI;
using UnityEngine;
using Stats;

namespace EnemyAI
{
    public class EnemyState
    {
        public Enemy enemy;
        public NavMeshAgent agent;
        public Player player;
        public Weapon currentWeapon;
        public EnemyAnimator enemyAnimator;
        public EnemyBehaviour enemyBehaviour;

        //States
        public IdleState idleState;
        public ChaseState chaseState;
        public AttackState attackState;
        public ParryState parryState;
        public StaggeredState staggeredState;
        public StunnedState stunnedState;
        public HitState hitState;
        public StandbyState standbyState;
        public BlockState blockState;
        public PerfectParryState perfectParryState;
        public ParryAttackState parryAttackState;

        public bool rotateTowardsPlayer { get; private set; }

        public void StartRotate()
        {
            rotateTowardsPlayer = true;
        }
        public void StopRotate()
        {
            rotateTowardsPlayer = false;
        }

        public virtual void Enter(Enemy enemy)
        {
            SetReferences(enemy);
        }
        public virtual void Update()
        {
            //Test for now, might delete this later
            if (rotateTowardsPlayer)
            {
                enemy.RotateToTarget(player.Position(), player.Position());
            }
        }
        public virtual void Staggered()
        {

        }
        public virtual void Stunned()
        {

        }
        public virtual void Hit()
        {
            
        }
        public virtual void OverlapCollider()
        {

        }
        public virtual void PlayerAttacking()
        {

        }
        public virtual void Dead()
        {
            enemy.StopMethod();
        }
        public virtual void Takedown()
        {

        }
        
        //Place this last as this might lead to stunned state before current state is done with enter
        public void TakeDamage()
        {
            enemy.health.TakeDamage(enemy.currentAttacker, enemy.hitPoint);
        }
        //Same with this as with TakeDamage
        public void ReturnPostureDamage(ParryType type)
        {
            enemy.parryCheck.ReturnPostureDamage(enemy.currentAttacker, enemy.hitPoint, type, -enemy.DirectionToTarget());
        }
        public void LeaveState(EnemyState newState)
        {
            enemy.StopMethod();
            enemy.SwitchState(newState);
        }
        public void LeaveStateAndDo(EnemyState newState, Action doThis)
        {
            doThis?.Invoke();
            LeaveState(newState);
        }
        private void SetReferences(Enemy enemy)
        {
            if (this.enemy == null)
            {
                this.enemy = enemy;
                agent = enemy.agent;
                player = enemy.player;
                currentWeapon = enemy.currentWeapon;
                enemyAnimator = enemy.enemyAnimator;
                enemyBehaviour = enemy.enemyBehaviour;

                idleState = enemy.idleState;
                chaseState = enemy.chaseState;
                attackState = enemy.attackState;
                parryState = enemy.parryState;
                staggeredState = enemy.staggeredState;
                stunnedState = enemy.stunnedState;
                hitState = enemy.hitState;
                standbyState = enemy.standbyState;
                blockState = enemy.blockState;
                perfectParryState = enemy.perfectParryState;
                parryAttackState = enemy.parryAttackState;

            }
        }
    }
}

