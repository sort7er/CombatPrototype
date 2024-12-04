using UnityEngine;

namespace EnemyAI
{

    public class StandbyState : EnemyState
    {
        public float waitTime;
        private bool turn;
        private float timer;
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            Anim standbyAnim = currentWeapon.archetype.enemyStandby;
            enemy.SetAnimation(standbyAnim, 0.25f);

            turn = false;
            timer = 0;

            //WaitTime is set here
            enemyBehaviour.StandbyEnter();
        }

        public override void Update()
        {
            if (turn)
            {
                // Actuall turning
                enemy.RotateToTarget(enemy.target.Position(), enemy.target.Position(), currentWeapon.archetype.enemyStandbyTurnRight.duration);
            }
            else
            {
                if (enemy.minTargetDistance < enemy.DistanceToTarget())
                {
                    LeaveStateAndDo(chaseState, LeaveStandby);
                }
                //Use this to determine if turn
                else if (enemy.InsideAttackFOV())
                {
                    timer += Time.deltaTime;

                    if (timer >= waitTime)
                    {

                        LeaveStateAndDo(attackState, LeaveStandby);
                    }

                    //This updates the rotation of the animation
                    enemy.SetLookAtAndForward(enemy.target.Position(), enemy.InFront());
                }
                else
                {
                    //For turn animation

                    float dot = Tools.Dot(enemy.DirectionToTarget(), enemy.Forward());

                    if (dot <= 0)
                    {
                        StartTurn(currentWeapon.archetype.enemyStandbyTurnRight);
                    }
                    else if (dot > 0)
                    {
                        StartTurn(currentWeapon.archetype.enemyStandbyTurnLeft);
                    }
                }
            }
        }

        private void StartTurn(Anim turnAnim)
        {
            turn = true;
            enemy.SetStartRotation(enemy.Rotation());
            enemy.StopMethod();
            enemy.SetAnimation(turnAnim);
            enemy.InvokeMethod(EndTurn, turnAnim.duration);

        }

        private void EndTurn()
        {
            turn = false;
        }
        public override void TargetAttacking()
        {
            enemyBehaviour.StandbyTargetAttack();
        }
        public override void Staggered()
        {
            LeaveStateAndDo(staggeredState, LeaveStandby);
        }
        public override void Stunned()
        {
            LeaveStateAndDo(stunnedState, LeaveStandby);
        }

        public override void Hit()
        {
            if (enemy.InsideParryFOV())
            {
                LeaveStateAndDo(parryState, LeaveStandby);
            }
            else
            {
                LeaveStateAndDo(hitState, LeaveStandby);
            }
        }
        public override void Takedown()
        {
            LeaveStateAndDo(takedownState, LeaveStandby);
        }

        public void LeaveStandby()
        {
            enemy.SetAnimationWithInt(enemy.attackDoneState);
        }
    }

    
}