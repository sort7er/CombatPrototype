using UnityEngine;

public class AttackState : EnemyState
{

    private bool coolDown;
    public override void EnterState(Enemy enemy)
    {
        enemy.DisableMovement();
        coolDown = false;
    }

    public override void UpdateState(Enemy enemy)
    {
        enemy.LookAtTarget(enemy.player.Position());

        if (HasArchetype(enemy))
        {
            if (Vector3.Distance(enemy.player.Position(), enemy.Position()) > enemy.playerDistance && !IsAttacking(enemy))
            {
                enemy.SwitchState(enemy.chaseState);
            }

            if (!IsAttacking(enemy) && !coolDown)
            {
                SelectCombo(enemy);
                coolDown = true;
                enemy.InvokeFunction(CooldownDone, Random.Range(enemy.attackCooldown, enemy.attackCooldown + 1));
            }
        }
        else
        {
            //If no archetype, go look for weapon or something. For now just chase player
            if (Vector3.Distance(enemy.player.Position(), enemy.Position()) > enemy.playerDistance)
            {
                enemy.SwitchState(enemy.chaseState);
            }
        }
    }

    private void SelectCombo(Enemy enemy)
    {
        int numberOfAttacks = Random.Range(1, 4);
            Debug.Log("Attack" + numberOfAttacks);

        for (int i = 0; i < numberOfAttacks; i++)
        {
            Debug.Log("Actual attack");
            RandomFire(enemy);
        }
    }

    private void RandomFire(Enemy enemy)
    {
        int rnd = Random.Range(0, 2);

        if (rnd == 0)
        {
            enemy.currentArchetype.archetypeAnimator.Fire();
        }
        else
        {
            enemy.currentArchetype.archetypeAnimator.HeavyFire();
        }
    }
    public void CooldownDone()
    {
        coolDown = false;
        Debug.Log("ye");
    }

    private bool HasArchetype(Enemy enemy)
    {
        if(enemy.currentArchetype != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsAttacking(Enemy enemy)
    {
        return enemy.currentArchetype.archetypeAnimator.isAttacking;
    }
}
