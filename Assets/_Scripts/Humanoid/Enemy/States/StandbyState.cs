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
                enemy.RotateToTarget(player.Position(), player.Position());
            }
            else
            {
                if (enemy.CheckView())
                {
                    LeaveStateAndDo(attackState, StandbyDone);
                }

                float dist = Vector3.Distance(player.Position(), enemy.Position());

                if (enemy.playerDistance < dist)
                {
                    LeaveStateAndDo(chaseState, StandbyDone);
                }


                enemy.SetLookAtAndForward(player.Position(), enemy.InFront());
                float dot = enemy.CalculateDotProduct();

                bool inFront = Tools.InFront(player.Position() - enemy.Position(), enemy.transform.right);


                if (dot > enemy.turnThreshold || !inFront)
                {
                    StartTurn(currentWeapon.archetype.enemyStandbyTurnRight);
                }
                else if (dot < -enemy.turnThreshold || !inFront)
                {
                    StartTurn(currentWeapon.archetype.enemyStandbyTurnLeft);
                }
            }
        }

        private void StartTurn(Anim turnAnim)
        {
            enemy.StopFunction();
            enemy.SetAnimation(turnAnim);
            turn = true;
            enemy.InvokeFunction(EndTurn, turnAnim.duration);

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