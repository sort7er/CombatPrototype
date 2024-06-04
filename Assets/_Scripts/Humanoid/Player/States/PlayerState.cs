using PlayerSM;
using System;
using UnityEngine;

namespace PlayerSM
{
    public enum QueuedAction
    {
        None,
        Attack,
        Block,
        Parry,
        Idle
    }


    public abstract class PlayerState
    {

        public QueuedAction upcommingAction;
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
        public virtual void Hit(Weapon attackingWeapon, Vector3 hitPoint)
        {

        }

        #endregion

        #region Queue methods
        public void QueueAttack(Action attackMethod)
        {
            if (actionDone)
            {
                attackMethod?.Invoke();
            }
            else if (CheckUpcommingAction())
            {
                SetUpcommingAction(QueuedAction.Attack);
            }

        }
        public void QueueBlock(Action blockMethod)
        {
            if (actionDone)
            {
                blockMethod?.Invoke();
            }
            else if (CheckUpcommingAction())
            {
                SetUpcommingAction(QueuedAction.Block);
            }
        }
        public void QueueIdle(Action idleMethod)
        {
            if (actionDone)
            {
                idleMethod?.Invoke();
            }
            else if (CheckUpcommingAction())
            {
                SetUpcommingAction(QueuedAction.Idle);
            }
        }
        public void QueueActionDone(Action attackMethod, Action blockMethod, Action idleMethod = null)
        {
            if (upcommingAction == QueuedAction.Attack)
            {
                attackMethod?.Invoke();
            }
            else if (upcommingAction == QueuedAction.Block)
            {
                blockMethod?.Invoke();
            }
            else if (upcommingAction == QueuedAction.Idle)
            {
                idleMethod?.Invoke();
            }
            else
            {
                actionDone = true;
            }
        }

        #endregion

        #region Tool methods
        public bool CheckUpcommingAction()
        {
            if (upcommingAction == QueuedAction.None && canChain)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public void SetUpcommingAction(QueuedAction action)
        {
            upcommingAction = action;
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
            SetUpcommingAction(QueuedAction.None);
        }
        public void ResetValues()
        {
            actionDone = false;
            canChain = true;
            SetUpcommingAction(QueuedAction.None);
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

