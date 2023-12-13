using System.Collections.Generic;
using UnityEngine;

public class Archetype : MonoBehaviour
{


    public string archetypeName;
    public ArchetypeAnimator archetypeAnimator;
    public TargetAssistanceParams targetAssistanceParams;
    public UniqueAbility uniqueAbility;

    //[Header("Target Assistance")]
    //public float range = 10;
    //public float idealDotProduct = 0.85f;
    //public float acceptedDotProduct = 0.75f;


    public void UniqueAttack(List<Enemy> enemies, PlayerData playerData)
    {
        if (enemies.Count > 0)
        {
            uniqueAbility.ExecuteAbility(playerData, targetAssistanceParams, enemies);
        }
        else
        {
            uniqueAbility.ExecuteAbilityNoTarget(playerData);
        }

        archetypeAnimator.UniqueFire();
    }

}
