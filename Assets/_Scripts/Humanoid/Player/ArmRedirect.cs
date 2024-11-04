using UnityEngine;

public class ArmRedirect : MonoBehaviour
{
    public Player player;

    public void ActionStart()
    {
        player.ActionStart();
    }
    public void OverlapCollider()
    {
        player.OverlapCollider();
        player.hitBox.OverlapCollider();
    }

    public void ActionDone()
    {
        player.ActionDone();
    }
    public void AbilityPing()
    {
        player.AbilityPing();
    }
}
