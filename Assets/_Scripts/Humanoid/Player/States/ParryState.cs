public class ParryState : ActionState
{
    private bool chain;
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
        if (!parry)
        {
            chain = true;
        }
        else
        {
            DoParry();
        }
    }

    private void DoParry()
    {
        chain = false;
        parry = false;
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
