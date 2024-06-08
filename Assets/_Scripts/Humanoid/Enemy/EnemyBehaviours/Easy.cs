using UnityEngine;

public class Easy : EnemyBehaviour
{
    #region Standby state
    public override void StandbyPlayerAttack()
    {
        Debug.Log("I don't do anything as I suck");
    }
    #endregion

    #region Block state
    public override void BlockHit()
    {
        Debug.Log("I don't do anything as I suck");
    }
    #endregion

    #region Parry state
    public override void ParryHit()
    {
        parryState.LeaveState(hitState);
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
        hitState.GetHit();
    }
    #endregion
}
