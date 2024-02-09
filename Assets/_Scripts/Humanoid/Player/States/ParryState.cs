public class ParryState : ActionState
{
    private bool chain;
    private bool attack;
    private bool parry;
    private int currentParry;
    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        DoParry();
    }

    public override void Parry()
    {
        if (!chain)
        {
            parry = true;
        }
        else
        {
            DoParry();
        }
    }

    public override void CheckChain()
    {
        if (attack)
        {
            actions.StopMethod();
            actions.SwitchState(actions.attackState);
        }
        else if (parry)
        {
            DoParry();
        }
        else
        {
            chain = true;
        }
    }

    public override void Attack()
    {
        if (chain)
        {
            actions.StopMethod();
            actions.SwitchState(actions.attackState);
        }
        else
        {
            attack= true;
        }
    }

    private void DoParry()
    {
        chain = false;
        parry = false;
        attack= false;
        actions.SetAnimation(archetype.parry[currentParry], 0.05f);
        actions.StopMethod();
        actions.InvokeMethod(EndParry, actions.currentAnimation.duration);
        UpdateParry();
    }

    private void EndParry()
    {
        actions.SwitchState(actions.idleState);
    }

    private void UpdateParry()
    {
        if (currentParry == 0)
        {
            currentParry = 1;
        }
        else
        {
            currentParry = 0;
        }
    }
}
