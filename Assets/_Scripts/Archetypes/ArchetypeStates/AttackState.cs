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
            numberOfAttacks = archetype.light.Length;
            archetypeAnimator = archetype;
            StartSettings();
            CheckAttack(archetypeAnimator.entryAttack);

        }
        public override void Fire(ArchetypeAnimator archetype)
        {
            UpdateCombo();
            CheckAttack(archetype.light[currentCombo]);
        }
        public override void HeavyFire(ArchetypeAnimator archetype)
        {
            UpdateCombo();
            CheckAttack(archetype.heavy[currentCombo]);
        }
        public override void UniqueFire(ArchetypeAnimator archetype)
        {
            CheckAttack(archetype.unique);
        }

        #region Unused
        public override void Block(ArchetypeAnimator archetype)
        {

        }
        public override void Parry(ArchetypeAnimator archetype)
        {

        }
        #endregion
        private void CheckAttack(Attack attack)
        {
            if (!archetypeAnimator.isAttacking)
            {
                Attack(attack);
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
        private void Attack(Attack attack, float crossfade = 0)
        {
            archetypeAnimator.SetCurrentAttack(attack);
            archetypeAnimator.IsAttacking(true);
            archetypeAnimator.CrossFade(attack, crossfade);

            float remapedValue = archetypeAnimator.Remap(archetypeAnimator.currentAttack.queuePoint);
            archetypeAnimator.InvokeFunction(CheckQueue, remapedValue);

            archetypeAnimator.InvokeFunction(AttackDone, archetypeAnimator.currentAttack.duration);            
        }

        private void CheckQueue()
        {
            // Attack if queue is not empty, there is an if statement here
            if (attackQueue.Count > 0)
            {
                archetypeAnimator.StopFunction();
                Attack(attackQueue[0], 0.25f);
                attackQueue.RemoveAt(0);
                archetypeAnimator.InvokeAttackDoneEvent();
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
            StartSettings();
            archetypeAnimator.InvokeAttackDoneEvent();
            archetypeAnimator.SwitchState(archetypeAnimator.idleState);
        }

        private void StartSettings()
        {
            currentCombo = 0;
            attackQueue.Clear();
            archetypeAnimator.IsAttacking(false);
            archetypeAnimator.SetCurrentAttack(null);
        }

    }
}