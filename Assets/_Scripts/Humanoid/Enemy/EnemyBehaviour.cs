using EnemyAI;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour: MonoBehaviour
{
    public Enemy enemy;

    protected IdleState idleState;
    protected ChaseState chaseState;
    protected AttackState attackState;
    protected ParryState parryState;
    protected StaggeredState staggeredState;
    protected StunnedState stunnedState;
    protected HitState hitState;
    protected StandbyState standbyState;
    protected BlockState blockState;
    protected PerfectParryState perfectParryState;


    #region Setup
    private void Start()
    {
        SetReferences(enemy);
    }

    private void SetReferences(Enemy enemy)
    {
        this.enemy = enemy;

        idleState = enemy.idleState;
        chaseState = enemy.chaseState;
        attackState = enemy.attackState;
        parryState = enemy.parryState;
        staggeredState = enemy.staggeredState;
        stunnedState = enemy.stunnedState;
        hitState = enemy.hitState;
        standbyState = enemy.standbyState;
        blockState = enemy.blockState;
        perfectParryState = enemy.perfectParryState;
    }

    #endregion

    #region Standby state
    public virtual void StandbyPlayerAttack()
    {

    }
    #endregion

    #region Block state
    public virtual void BlockHit()
    {

    }
    #endregion

    #region Parry state
    public virtual void ParryHit()
    {

    }
    #endregion

}
