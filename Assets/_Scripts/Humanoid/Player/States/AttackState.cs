using Actions;
public class AttackState : ActionState
{
    private int currentAttack;

    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        currentAttack = 0;
        StartAttack();
    }

    public override void Attack()
    {
        if(actionDone)
        {
            StartAttack();
        }
        else if (canChain && CheckUpcommingAction())
        {
            SetUpcommingAction(QueuedAction.Attack);
        }

    }

    public override void ActionDone()
    {
        if(upcommingAction == QueuedAction.Attack)
        {
            StartAttack();
        }
        else if(upcommingAction == QueuedAction.Block)
        {
            actions.StopMethod();
            actions.SwitchState(actions.blockState);
        }
        else
        {
            actionDone= true;
        }
    }

    public override void Block()
    {
        if(actionDone)
        {
            actions.StopMethod();
            actions.SwitchState(actions.blockState);
        }
        else if(canChain && CheckUpcommingAction())
        {
            SetUpcommingAction(QueuedAction.Block);
        }
    }

    public override void Parry()
    {
        if(upcommingAction == QueuedAction.Block)
        {
            actions.StopMethod();
            actions.SwitchState(actions.parryState);
        }
    }

    private void StartAttack()
    {
        actionDone = false;
        canChain= false;
        SetUpcommingAction(QueuedAction.None);
        actions.SetAnimation(archetype.attacks[currentAttack], 0.05f);
        actions.StopMethod();
        actions.InvokeMethod(EndAttack, actions.currentAnimation.duration);
        UpdateAttack();
    }


    private void EndAttack()
    {
        actions.SwitchState(actions.idleState);
    }

    private void UpdateAttack()
    {
        if(currentAttack < archetype.attacks.Length - 1)
        {
            currentAttack++;
        }
        else
        {
            currentAttack = 0;
        }
    }

}
