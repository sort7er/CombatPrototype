using UnityEngine;

public class AttackState : EnemyState
{
    public override void EnterState(Enemy enemy)
    {
        enemy.DisableMovement();
    }

    public override void UpdateState(Enemy enemy)
    {
        enemy.LookAtTarget(enemy.player.Position());
        if (Vector3.Distance(enemy.player.Position(), enemy.Position()) > enemy.playerDistance)
        {
            enemy.SwitchState(enemy.chaseState);
        }
    }
}
