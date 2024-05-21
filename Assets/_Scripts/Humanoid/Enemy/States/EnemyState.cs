using UnityEngine.AI;

namespace EnemyAI
{
    public class EnemyState
    {
        public Enemy enemy;
        public NavMeshAgent agent;
        public Player player;
        public Weapon currentWeapon;

        public virtual void Enter(Enemy enemy)
        {
            SetReferences(enemy);
        }
        public virtual void Update()
        {

        }
        public virtual void Staggered()
        {

        }
        public virtual void Stunned()
        {

        }
        public virtual void Hit()
        {

        }
        public virtual void Takedown()
        {

        }
        private void SetReferences(Enemy enemy)
        {
            if (this.enemy == null)
            {
                this.enemy = enemy;
                agent = enemy.agent;
                player = enemy.player;
                currentWeapon = enemy.currentWeapon;
            }
        }
    }
}

