using UnityEngine;

public class TakedownState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        Debug.Log("wd");
        enemy.ImmediateStop();
    }

    public override void UpdateState(Enemy enemy)
    {

    }
}
