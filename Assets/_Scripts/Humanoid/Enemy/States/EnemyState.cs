using System;
using UnityEngine.AI;
using UnityEngine;

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

        public virtual void Enter(Enemy enemy)
        {
            SetReferences(enemy);
        }
        public virtual void Update()
        {

        }
        public virtual void Staggered()
        {

        }
        public virtual void Stunned()
        {

        }
        public virtual void Hit(Weapon attackingWeapon, Vector3 hitPoint)
        {
            enemy.health.TakeDamage(attackingWeapon, hitPoint);
        }
        public virtual void OverlapCollider()
        {

        }
        public virtual void PlayerAttacking()
        {

        }
        public virtual void Dead()
        {
            enemy.StopFunction();
        }
        public virtual void Takedown()
        {

        }

        public void LeaveState(EnemyState newState)
        {
            enemy.StopFunction();
            enemy.SwitchState(newState);
        }
        public void LeaveStateAndDo(EnemyState newState, Action doThis)
        {
            doThis?.Invoke();
            enemy.StopFunction();
            enemy.SwitchState(newState);
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

            }
        }
    }
}

