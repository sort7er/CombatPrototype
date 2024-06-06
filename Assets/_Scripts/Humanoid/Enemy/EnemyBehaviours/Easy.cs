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

    #endregion

    #region Parry state
    public override void ParryHit()
    {
        parryState.LeaveState(hitState);
    }
    #endregion

    #region Perfect parry state
    public override void PerfectParryEnter()
    {

    }
    #endregion
}
