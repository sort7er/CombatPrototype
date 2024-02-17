using Actions;
using System.Collections.Generic;

public class UniqueState : ActionState
{
    private List<Enemy> enemyList;
    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        actions.SetAnimation(archetype.unique);
        ClearList();
        enemyList = actions.player.targetAssistance.CheckForEnemies(actions.currentWeapon.uniqueAbility);


        if (enemyList.Count > 0)
        {
            actions.currentWeapon.uniqueAbility.ExecuteAbility(actions.player, enemyList);
        }
        else
        {
            actions.currentWeapon.uniqueAbility.ExecuteAbilityNoTarget(actions.player);
        }

        actions.InvokeMethod(EndAttack, actions.currentAnimation.duration);

        actionDone = false;
        canChain = false;
        SetUpcommingAction(QueuedAction.None);


    }

    public override void Attack()
    {
        if (actionDone)
        {
            actions.StopMethod();
            actions.SwitchState(actions.attackState);
        }
        else if (canChain && CheckUpcommingAction())
        {
            SetUpcommingAction(QueuedAction.Attack);
        }

    }
    public override void Block()
    {
        if (actionDone)
        {
            actions.StopMethod();
            actions.SwitchState(actions.blockState);
        }
        else if (canChain && CheckUpcommingAction())
        {
            SetUpcommingAction(QueuedAction.Block);
        }
    }

    public override void Parry()
    {
        if (upcommingAction == QueuedAction.Block)
        {
            actions.StopMethod();
            actions.SwitchState(actions.parryState);
        }
    }

    public override void ActionDone()
    {
        if (upcommingAction == QueuedAction.Attack)
        {
            actions.StopMethod();
            actions.SwitchState(actions.attackState);
        }
        else if (upcommingAction == QueuedAction.Block)
        {
            actions.StopMethod();
            actions.SwitchState(actions.blockState);
        }
        else
        {
            actionDone = true;
        }
    }

    private void EndAttack()
    {
        actions.SwitchState(actions.idleState);
    }

    private void ClearList()
    {
        if(enemyList == null)
        {
            enemyList = new();
        }
        enemyList.Clear();
    }
}
