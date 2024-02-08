using UnityEngine;

public class ParryState : ActionState
{
    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        Debug.Log("Parry");
    }

}
