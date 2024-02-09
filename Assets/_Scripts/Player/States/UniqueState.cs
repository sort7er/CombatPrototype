using UnityEngine;

public class UniqueState : ActionState
{
    public override void Enter(PlayerActions actions)
    {
        base.Enter(actions);
        Debug.Log("Unique");
        actions.SetAnimation(archetype.unique);
    }

}
