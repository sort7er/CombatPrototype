using System;
using UnityEngine;

namespace PlayerSM
{
    public abstract class PlayerState
    {
        public Player player;
        public Weapon weapon;
        public Archetype archetype;
        public bool canChain;
        public bool actionDone;

        public IdleState idleState;
        public JumpState jumpState;
        public FallState fallState;
        public AttackState attackState;
        public UniqueState uniqueState;
        public BlockState blockState;
        public ParryState parryState;
        public PerfectParryState perfectParryState;
        public ParryAttackState parryAttackState;
        public StaggeredState staggeredState;
        public HitState hitState;

        private Action nextAction;

        #region Signal methods
        public virtual void Enter(Player player)
        {
            SetReferences(player);
        }
        public virtual void Update()
        {

        }
        public virtual void Moving()
        {

        }
        public virtual void StoppedMoving()
        {

        }
        public virtual void Jump()
        {

        }
        public virtual void Fall()
        {

        }
        public virtual void Landing()
        {

        }
        public virtual void Attack()
        {

        }
        public virtual void OverlapCollider()
        {
            canChain = true;
        }
        public virtual void ActionDone()
        {
            CheckQueueOrActionDone();
        }
        public virtual void Unique()
        {

        }
        public virtual void Block()
        {

        }
        public virtual void BlockRelease()
        {

        }
        public virtual void Parry()
        {

        }
        public virtual void PerfectParry()
        {

        }
        public virtual void Staggered()
        {
            
        }
        public virtual void Hit()
        {

        }

        #endregion

        #region Queue methods
        public void DoOrQueueAction(Action action)
        {
            if (actionDone)
            {
                action?.Invoke();
            }
            else
            {
                CheckNextAction(action);
            }
        }

        public void CheckQueueOrActionDone()
        {
            if(nextAction != null)
            {
                nextAction?.Invoke();
            }
            else
            {
                actionDone = true;
            }
        }

        #endregion

        #region Tool methods
        private void CheckNextAction(Action action)
        {
            if(nextAction == null)
            {
                nextAction = action;
            }
        }

        public void LeaveState(PlayerState newState)
        {
            player.StopMethod();
            player.SwitchState(newState);
        }
        public void LeaveStateAndDo(PlayerState newState, Action doThis)
        {
            doThis?.Invoke();
            player.StopMethod();
            player.SwitchState(newState);
        }

        public void ResetValuesAttack()
        {
            actionDone = false;
            canChain = false;
            nextAction = null;
        }
        public void ResetValues()
        {
            actionDone = false;
            canChain = true;
            nextAction = null;
        }
        #endregion

        #region Private methods
        private void SetReferences(Player player)
        {
            if(this.player == null)
            {
                this.player = player;
                idleState = player.idleState;
                jumpState = player.jumpState;
                fallState = player.fallState;
                attackState = player.attackState;
                uniqueState = player.uniqueState;
                blockState = player.blockState;
                parryState = player.parryState;
                perfectParryState = player.perfectParryState;
                parryAttackState = player.parryAttackState;
                staggeredState = player.staggeredState;
                hitState = player.hitState;
            }

            //Need to update the current weapon all the time to make sure the state knows which weapon is in use
            UpdateWeapon(player.currentWeapon);
        }

        public void UpdateWeapon(Weapon weapon)
        {
            this.weapon = weapon;
            archetype = weapon.archetype;
        }
        #endregion
    }

}

