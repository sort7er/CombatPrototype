using System.Collections.Generic;
using UnityEngine;

namespace ArchetypeStates
{
    public class AttackState : ArchetypeState
    {
        public List<Attack> attackQueue = new();
        private ArchetypeAnimator archetypeAnimator;

        public int currentCombo;


        public override void EnterState(ArchetypeAnimator archetype, Animator anim)
        {
            archetypeAnimator = archetype;
            StartSettings();
            CheckAttack(archetypeAnimator.entryAttack);
        }
        public override void Fire(ArchetypeAnimator archetype)
        {
            CheckAttack(archetype.light[currentCombo]);
        }
        public override void HeavyFire(ArchetypeAnimator archetype)
        {
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
            if (currentCombo < 3)
            {
                attackQueue.Add(attack);
                UpdateCombo();
            }
        }
        private void Attack(Attack attack, float crossfade = 0)
        {
            UpdateCombo();
            archetypeAnimator.SetCurrentAttack(attack);
            archetypeAnimator.IsAttacking(true);
            archetypeAnimator.SetAnimation(attack, crossfade);

            float remapedValue = Remap(archetypeAnimator.currentAttack.queuePoint, 0, 60, 0, 1);
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
            if (currentCombo < 2)
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
        //Tool to mach invoke time with keyframes
        private float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return from2 + (value - from1) * (to2 - from2) / (to1 - from1);
        }
    }
}