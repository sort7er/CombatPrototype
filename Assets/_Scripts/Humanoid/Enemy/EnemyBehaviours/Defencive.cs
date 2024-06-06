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

    #region Parry state
    public override void ParryHit()
    {       
        if (enemy.InsideParryFOV())
        {
            enemy.parryState.LeaveState(perfectParryState);
        }
        else
        {
            parryState.SwitchToHit();
        }
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
}
