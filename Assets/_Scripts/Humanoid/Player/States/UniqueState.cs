using System.Collections.Generic;

public class UniqueState : ActionState
{
    private List<Enemy> enemyList;
    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        actions.SetAnimation(archetype.unique);
        ClearList();
        enemyList = actions.player.targetAssistance.CheckForEnemies(actions.currentWeapon.uniqueAbility);


        if (enemyList.Count > 0)
        {
            actions.currentWeapon.uniqueAbility.ExecuteAbility(actions.player, enemyList);
        }
        else
        {
            actions.currentWeapon.uniqueAbility.ExecuteAbilityNoTarget(actions.player);
        }

        actions.InvokeMethod(EndAttack, actions.currentAnimation.duration);

    }


private void EndAttack()
{
    actions.SwitchState(actions.idleState);
}

private void ClearList()
    {
        if(enemyList == null)
        {
            enemyList = new();
        }
        enemyList.Clear();
    }
}
