using UnityEngine;

public class ArmRedirect : MonoBehaviour
{
    public Player player;
    public void OverlapCollider()
    {
        player.playerActions.OverlapCollider();
        player.hitBox.OverlapCollider();
    }
    public void ActionDone()
    {
        player.playerActions.ActionDone();
    }
}
