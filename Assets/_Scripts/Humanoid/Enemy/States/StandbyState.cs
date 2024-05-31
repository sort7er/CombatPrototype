using System;
using UnityEngine;

namespace EnemyAI
{

    public class StandbyState : EnemyState
    {
        private bool turn;
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            Anim standbyAnim = currentWeapon.archetype.enemyStandby;

            enemy.SetAnimation(standbyAnim);

            enemyAnimator.SetAnimatorBool("Standby", true);

            turn = false;
        }

        public override void Update()
        {

            if (turn)
            {
                enemy.RotateToTarget(player.Position());
            }
            else
            {
                enemy.SetLookAtPos(player.Position());
                float dot = enemy.CalculateDotProduct();

                if (dot + enemy.turnThreshold < 0)
                {
                    StartTurn(currentWeapon.archetype.enemyStandbyTurnRight);
                }
                else if (dot - enemy.turnThreshold > 0)
                {
                    StartTurn(currentWeapon.archetype.enemyStandbyTurnLeft);
                }
            }
        }

        private void StartTurn(Anim turnAnim)
        {
            enemy.StopFunction();
            //enemy.SetAnimation(turnAnim);

            enemy.InvokeFunction(DoTurn, turnAnim.duration * 0.45f);
            enemy.InvokeFunction(EndTurn, turnAnim.duration);

        }

        private void DoTurn()
        {
            turn = true;
        }

        private void EndTurn()
        {
            turn = false;
        }

        public override void Hit()
        {
            LeaveStateAndDo(hitState, StandbyDone);
        }

        private void StandbyDone()
        {
            enemyAnimator.SetAnimatorBool("Standby", false);
        }
    }

    
}