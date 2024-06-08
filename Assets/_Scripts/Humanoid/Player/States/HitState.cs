using UnityEngine;
namespace PlayerSM
{
    public class HitState : PlayerState
    {
        public override void Enter(Player player)
        {
            base.Enter(player);
            Debug.Log("Now I am hit");
        }
        public override void Hit()
        {
            Debug.Log("Damn, hit again");
        }
    }
}