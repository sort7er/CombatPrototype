using UnityEngine;

public class AttackState : ActionState
{
    private bool chain;
    private bool attack;
    private int currentAttack;

    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        currentAttack = 0;
        StartAttack();
    }

    public override void Attack()
    {
        if(!chain)
        {
            attack= true;
        }
        else
        {
            StartAttack();
        }
    }

    public override void CheckChain()
    {
        if(!attack)
        {
            chain= true;
        }
        else
        {
            StartAttack();
        }
    }

    private void StartAttack()
    {
        chain = false;
        attack= false;
        actions.SetAnimation(archetype.attacks[currentAttack], 0.1f);
        actions.StopMethod();
        actions.InvokeMethod(AttackDone, actions.currentAnimation.duration);
        UpdateAttack();
    }


    private void AttackDone()
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
