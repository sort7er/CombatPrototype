using UnityEngine;

public class ArmRedirect : MonoBehaviour
{
    public PlayerActions actions;
    public void OverlapCollider()
    {
        actions.OverlapCollider();
    }
    public void ActionDone()
    {
        actions.ActionDone();
    }
}
