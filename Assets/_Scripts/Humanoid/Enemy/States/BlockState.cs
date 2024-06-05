using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EnemyAI
{
    public class BlockState : EnemyState
    {
        public override void Enter(Enemy enemy)
        {
            base.Enter(enemy);
            Debug.Log("Now I am blocking");
        }
    }
}
