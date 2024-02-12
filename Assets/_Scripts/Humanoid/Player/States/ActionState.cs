using Actions;

namespace Actions
{
    public enum QueuedAction
    {
        None,
        Attack,
        Block,
        Parry
    }

}

public abstract class ActionState
{

    public QueuedAction upcommingAction;
    public PlayerActions actions;
    public Weapon weapon;
    public Archetype archetype;
    public bool canChain;
    public bool actionDone;

    public virtual void Enter(PlayerActions actions)
    {
        SetReferences(actions);
    }
    public virtual void Update()
    {

    }
    public virtual void Moving()
    {

    }
    public virtual void StoppedMoving()
    {

    }
    public virtual void Jump()
    {

    }
    public virtual void Fall()
    {

    }
    public virtual void Landing()
    {

    }
    public virtual void Attack()
    {

    }
    public virtual void OverlapCollider()
    {
        canChain = true;
    }
    public virtual void ActionDone()
    {

    }
    public virtual void Unique()
    {

    }
    public virtual void Block()
    {

    }
    public virtual void Parry()
    {

    }

    public void SetReferences(PlayerActions actions)
    {
        if(this.actions == null)
        {
            this.actions = actions;
            weapon = actions.currentWeapon;
            archetype = actions.currentWeapon.archetype;
        }
    }

    public bool CheckUpcommingAction()
    {
        if(upcommingAction == QueuedAction.None)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void SetUpcommingAction(QueuedAction action)
    {
        upcommingAction = action;
    }
}
