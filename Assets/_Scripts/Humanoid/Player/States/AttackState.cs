using UnityEngine;

public class AttackState : ActionState
{
    private bool chain;
    private bool attack;
    private bool block;
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
        if(attack)
        {
            StartAttack();
        }
        else if (block)
        {
            actions.StopMethod();
            actions.SwitchState(actions.blockState);
        }
        else
        {
            chain= true;
        }
    }

    public override void Block()
    {
        if(chain)
        {
            actions.StopMethod();
            actions.SwitchState(actions.blockState);
        }
        else
        {
            block= true;
        }
    }

    public override void Parry()
    {
        if(block)
        {
            actions.StopMethod();
            actions.SwitchState(actions.parryState);
        }
    }

    private void StartAttack()
    {
        chain = false;
        attack= false;
        block= false; 
        actions.SetAnimation(archetype.attacks[currentAttack], 0.15f);
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
