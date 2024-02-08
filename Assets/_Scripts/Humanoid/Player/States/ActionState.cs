public abstract class ActionState
{
    public PlayerActions actions;
    public Weapon weapon;
    public Archetype archetype;
    public virtual void Enter(PlayerActions actions)
    {
        SetReferences(actions);
    }
    public virtual void Update()
    {

    }
    public virtual void Attack()
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
}
