using UnityEngine;

public class Defencive : EnemyBehaviour
{

    #region Standby state
    public override void StandbyPlayerAttack()
    {
        Debug.Log("Here I " + enemy + ", should block as I am in the " + enemy.currentState + " state");
    }
    #endregion

}
