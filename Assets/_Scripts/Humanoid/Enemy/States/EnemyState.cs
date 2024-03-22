using UnityEngine.AI;

namespace EnemyAI
{
    public class EnemyState
    {
        public Enemy enemy;
        public NavMeshAgent agent;
        public Player player;

        public virtual void Enter(Enemy enemy)
        {
            SetReferences(enemy);
        }
        public virtual void Update()
        {

        }
        private void SetReferences(Enemy enemy)
        {
            if (this.enemy == null)
            {
                this.enemy = enemy;
                agent = enemy.agent;
                player = enemy.player;
            }
        }
    }
}

