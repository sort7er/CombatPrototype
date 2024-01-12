using UnityEngine;

namespace ArchetypeStates
{
    public class AttackState : ArchetypeState
    {
        public override void EnterState(ArchetypeAnimator archetype, Animator anim)
        {

        }
        public override void Fire(ArchetypeAnimator archetype)
        {

        }
        public override void HeavyFire(ArchetypeAnimator archetype)
        {

        }
        public override void UniqueFire(ArchetypeAnimator archetype)
        {

        }
        public override void Block(ArchetypeAnimator archetype)
        {

        }
        public override void Parry(ArchetypeAnimator archetype)
        {

        }
        //private void CheckAction(ActionType actionType)
        //{
        //    if (!isAttacking && !isDefending)
        //    {
        //        //If not currently using the sword

        //        if (actionType == ActionType.block || actionType == ActionType.parry)
        //        {
        //            Defence(actionType);
        //        }
        //        else
        //        {
        //            Attack(actionType);
        //        }
        //    }
        //    else if (isDefending && actionType == ActionType.parry)
        //    {
        //        Defence(actionType);
        //    }
        //    else if (isAttacking)
        //    {
        //        //If is currently attacking, try and add this attack to queue. Cannot queue unique attack
        //        if (actionType == ActionType.light || actionType == ActionType.heavy)
        //        {
        //            TryAddToQueue(actionType);
        //        }

        //    }

        //}
    }
}