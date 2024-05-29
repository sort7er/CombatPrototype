using System;
using UnityEngine;

namespace EnemyAI
{

    public class StandbyState : EnemyState
    {
        private float turnThreshold = 0.5f;
        private bool turn;
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            Anim standbyAnim = currentWeapon.archetype.enemyStandby;

            enemy.SetAnimation(standbyAnim);

            enemyAnimator.SetAnimatorBool("Standby", true);

            turn = false;
            enemy.SetWalkToTarget(player.Position());
        }

        public override void Update()
        {
            enemy.SetLookAtTarget(player.Position());



            if(turn)
            {
                enemy.SetWalkToTarget(player.Position());
                enemy.RotateToTarget();
            }
            else
            {
                float dot = enemy.CalculateDotProduct();

                if (dot + turnThreshold < 0)
                {
                    StartTurn(currentWeapon.archetype.enemyStandbyTurnRight);
                }
                else if (dot - turnThreshold > 0)
                {
                    StartTurn(currentWeapon.archetype.enemyStandbyTurnLeft);
                }
            }
        }

        private void StartTurn(Anim turnAnim)
        {
            enemy.StopFunction();
            enemy.SetAnimation(turnAnim);

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