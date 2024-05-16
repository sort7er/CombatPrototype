using Actions;
using System;
using UnityEngine;

namespace Actions
{
    public enum QueuedAction
    {
        None,
        Attack,
        Block,
        Parry,
        Idle
    }


    public abstract class ActionState
    {

        public QueuedAction upcommingAction;
        public PlayerActions actions;
        public Weapon weapon;
        public Archetype archetype;
        public bool canChain;
        public bool actionDone;

        #region Signal methods
        public virtual void Enter(PlayerActions actions)
        {
            SetReferences(actions);
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

        public void LeaveState(ActionState newState)
        {
            actions.StopMethod();
            actions.SwitchState(newState);
        }
        public void LeaveStateAndDo(ActionState newState, Action doThis)
        {
            doThis?.Invoke();
            actions.StopMethod();
            actions.SwitchState(newState);
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
        private void SetReferences(PlayerActions actions)
        {
            this.actions = actions;
            weapon = actions.currentWeapon;
            archetype = actions.currentWeapon.archetype;
        }
        #endregion
    }

}

