using UnityEngine;

namespace PlayerSM
{
    public class RangedState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);
            player.InvokeMethod(EndRanged, 1f);
            Debug.Log("Entered ranged");
        }




        private void EndRanged()
        {
            Debug.Log("Ranged left");
            LeaveState(idleState);
        }
    }
}
