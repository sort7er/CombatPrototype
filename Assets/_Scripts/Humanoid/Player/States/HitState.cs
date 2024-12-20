using UnityEngine;
namespace PlayerSM
{
    public class HitState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);
            ResetValues();
            Anim hitAnim = weapon.archetype.hit;

            player.SetAnimation(hitAnim,0);
            player.InvokeMethod(HitDone, hitAnim.duration);
        }

        public override void Attack()
        {
            DoOrQueueAttack(() => LeaveState(attackState));
        }
        public override void Block()
        {
            DoOrQueueBlock(() => LeaveState(blockState));
        }

        private void HitDone()
        {
            LeaveState(idleState);
        }

        public override void Hit()
        {
            Debug.Log("Damn, hit again");
        }
        public override void Stunned()
        {
            LeaveState(stunnedState);
        }
    }
}