using UnityEngine;

namespace EnemyAI
{

    public class StandbyState : EnemyState
    {
        private bool turn;
        private float waitTime;
        private float timer;
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);

            Anim standbyAnim = currentWeapon.archetype.enemyStandby;
            enemy.SetAnimation(standbyAnim, 0.25f);

            turn = false;
            timer = 0;
            waitTime = Random.Range(1, 2);
        }

        public override void Update()
        {
            if (turn)
            {
                // Actuall turning
                enemy.RotateToTarget(player.Position(), player.Position());
            }
            else
            {
                if (enemy.minPlayerDistance < enemy.DistanceToTarget())
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
                    enemy.SetLookAtAndForward(player.Position(), enemy.InFront());
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
            enemy.StopFunction();
            enemy.SetAnimation(turnAnim);
            enemy.InvokeFunction(EndTurn, turnAnim.duration);

        }

        private void EndTurn()
        {
            turn = false;
        }

        public override void Hit(Weapon attackingWeapon, Vector3 hitPoint)
        {
            if (enemy.InsideParryFOV())
            {
                LeaveStateAndDo(parryState, LeaveStandby);
            }
            else
            {
                base.Hit(attackingWeapon, hitPoint);
                LeaveStateAndDo(hitState, LeaveStandby);
            }
        }

        private void LeaveStandby()
        {
            enemyAnimator.animator.CrossFade(enemy.attackDoneState, 0);
        }
    }

    
}