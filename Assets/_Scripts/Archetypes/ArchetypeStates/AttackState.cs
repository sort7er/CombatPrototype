using System.Collections.Generic;
using UnityEngine;

namespace ArchetypeStates
{
    public class AttackState : ArchetypeState
    {
        public List<Attack> attackQueue = new();
        private ArchetypeAnimator archetypeAnimator;

        public int currentCombo;

        // Heavy and light must match
        private int numberOfAttacks; 


        public override void EnterState(ArchetypeAnimator archetype)
        {
            StartSettings(archetype);
            CheckAttack(archetype, archetypeAnimator.entryAttack);

        }
        public override void Fire(ArchetypeAnimator archetype)
        {
            UpdateCombo();
            CheckAttack(archetype, archetype.light[currentCombo]);
        }
        public override void HeavyFire(ArchetypeAnimator archetype)
        {
            UpdateCombo();
            CheckAttack(archetype, archetype.heavy[currentCombo]);
        }
        public override void UniqueFire(ArchetypeAnimator archetype)
        {
            CheckAttack(archetype, archetype.unique);
        }

        #region Unused
        public override void Block(ArchetypeAnimator archetype)
        {

        }
        public override void Parry(ArchetypeAnimator archetype)
        {

        }

        #endregion
        public override void Staggered(ArchetypeAnimator archetype)
        {
            archetypeAnimator.StopFunction();
            ResetQueue();
            archetypeAnimator.AttackingDone();
            archetypeAnimator.SwitchState(archetypeAnimator.staggeredState);
        }
        private void CheckAttack(ArchetypeAnimator archetype, Attack attack)
        {
            if (!archetype.isAttacking)
            {
                Attack(archetype, attack);
            }
            else
            {
                //Dont queue the attack if it is unique
                if(attack != archetypeAnimator.unique)
                {
                    TryAddToQueue(attack);
                } 
            }
        }
        private void TryAddToQueue(Attack attack)
        {
            // If there is capasity in the queue, add the new attack
            if (currentCombo < numberOfAttacks)
            {
                attackQueue.Add(attack);
            }
        }
        private void Attack(ArchetypeAnimator archetype, Attack attack, float crossfade = 0)
        {
            archetype.IsAttacking(attack, crossfade);

            float remapedValue = archetype.Remap(archetype.currentAttack.queuePoint);
            archetype.InvokeFunction(CheckQueue, remapedValue);

            archetype.InvokeFunction(AttackDone, archetype.currentAttack.duration);            
        }

        private void CheckQueue()
        {
            // Attack if queue is not empty, there is an if statement here
            if (attackQueue.Count > 0)
            {
                archetypeAnimator.StopFunction();
                archetypeAnimator.SwingDone();
                Attack(archetypeAnimator, attackQueue[0], 0.1f);
                attackQueue.RemoveAt(0);
            }
        }

        private void UpdateCombo()
        {
            if (currentCombo < numberOfAttacks - 1)
            {
                currentCombo++;
            }
        }
        private void AttackDone()
        {
            ResetQueue();
            archetypeAnimator.AttackingDone();
            archetypeAnimator.SwitchState(archetypeAnimator.idleState);
        }

        private void StartSettings(ArchetypeAnimator archetype)
        {
            numberOfAttacks = archetype.light.Length;
            archetypeAnimator = archetype;
            ResetQueue();
            archetypeAnimator.NotAttacking();
        }
        private void ResetQueue()
        {
            currentCombo = 0;
            attackQueue.Clear();
        }

    }
}