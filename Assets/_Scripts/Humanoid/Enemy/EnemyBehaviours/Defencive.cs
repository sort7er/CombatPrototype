using UnityEngine;

public class Defencive : EnemyBehaviour
{

    #region Standby state
    public override void StandbyPlayerAttack()
    {
        standbyState.LeaveStateAndDo(blockState, standbyState.LeaveStandby);
    }
    #endregion

    #region Block state
    public override void BlockHit()
    {
        blockState.LeaveStateAndDo(parryState, blockState.LeaveBlocking);
    }
    #endregion

}
