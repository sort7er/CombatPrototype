using Stats;
using UnityEngine;

public class Defencive : EnemyBehaviour
{

    #region Standby state
    public override void StandbyEnter()
    {
        standbyState.waitTime = Random.Range(1, 2);
    }
    public override void StandbyTargetAttack()
    {
        standbyState.LeaveStateAndDo(blockState, standbyState.LeaveStandby);
    }
    #endregion
    #region Attack state
    public override void AttackTargetAttack()
    {
        if (attackState.canParry)
        {
            attackState.LeaveStateAndDo(blockState, () => attackState.LeaveAttack());
        }
    }
    #endregion
    #region Block state
    public override void BlockHit()
    {
        blockState.LeaveStateAndDo(enemy.nextParryState, blockState.LeaveBlocking);
    }
    #endregion

    #region Parry state
    public override void ParryEnter()
    {
        parryState.DoParry();
        
        float duration = enemy.currentWeapon.archetype.enemyParrys[parryState.currentParry].duration;
        enemy.InvokeMethod(parryState.EndParry, duration * 0.6f);
    }
    public override void ParryHit()
    {       
        if (enemy.InsideParryFOV())
        {
            parryState.LeaveState(perfectParryState);
        }
        else
        {
            parryState.LeaveState(hitState);
        }
    }
    public override void ParryTargetAttack()
    {
        parryState.LeaveStateAndDo(blockState, () => enemy.SetNextParryState(perfectParryState));
    }
    public override void ParryEnd()
    {
        parryState.LeaveState(attackState);
    }
    #endregion

    #region Perfect parry state
    public override void PerfectParryEnter()
    {
        Anim perfectParry = enemy.currentWeapon.archetype.enemyPerfectParry;

        float waitTime = perfectParry.duration * 0.3f;

        enemy.InvokeMethod(perfectParryState.DoPefectParryAttack, waitTime);
    }
    #endregion

    #region Hit state
    public override void HitHit()
    {
        if (enemy.InsideParryFOV())
        {
            hitState.LeaveState(parryState);
        }
        else
        {
            hitState.GetHit();
        }
    }
    #endregion
    #region Takedownstate
    public override void TakedownEnter()
    {
        takedownState.LeaveState(blockState);
    }
    #endregion
}
