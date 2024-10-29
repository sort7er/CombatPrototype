using UnityEngine;


namespace PlayerSM
{
    public class MeleeState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);
            player.InvokeMethod(EndMelee, 1f);
            Debug.Log("Entered melee");

            Attack melee = weapon.abilitySet.melee;
            Debug.Log(melee);

        }

        private void EndMelee()
        {
            Debug.Log("Melee left");
            LeaveState(idleState);
        }
    }
}
