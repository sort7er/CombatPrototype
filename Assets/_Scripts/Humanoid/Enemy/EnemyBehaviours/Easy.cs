using UnityEngine;

public class Easy : EnemyBehaviour
{
    #region Standby state
    public override void StandbyEnter()
    {
        standbyState.waitTime = 0.2f;
    }
    public override void StandbyTargetAttack()
    {
        Debug.Log("I don't do anything as I suck");
    }
    #endregion
    #region Attack state
    public override void AttackTargetAttack()
    {

    }
    #endregion
    #region Block state
    public override void BlockHit()
    {
        Debug.Log("I don't do anything as I suck");
    }
    #endregion

    #region Parry state

    public override void ParryEnter()
    {
        parryState.DoParry();

        float duration = enemy.currentWeapon.archetype.enemyParrys[parryState.currentParry].duration;
        enemy.InvokeMethod(parryState.EndParry, duration);
    }
    public override void ParryHit()
    {
        parryState.LeaveState(hitState);
    }
    public override void ParryTargetAttack()
    {

    }
    public override void ParryEnd()
    {
        parryState.LeaveState(standbyState);
    }
    #endregion

    #region Perfect parry state
    public override void PerfectParryEnter()
    {

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

    }
    #endregion
}
