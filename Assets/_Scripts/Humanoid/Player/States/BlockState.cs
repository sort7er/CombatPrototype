using UnityEngine;

public class BlockState : ActionState
{
    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        Debug.Log("Block");
    }

}
