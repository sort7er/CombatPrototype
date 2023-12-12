using System.Collections.Generic;
using UnityEngine;

public class Archetype : MonoBehaviour
{


    public string archetypeName;
    public ArchetypeAnimator archetypeAnimator;
    public TargetAssistanceParams targetAssistanceParams;
    public UniqueAbility uniqueAbility { get; private set; }

    [SerializeField] private Type archetype;

    //[Header("Target Assistance")]
    //public float range = 10;
    //public float idealDotProduct = 0.85f;
    //public float acceptedDotProduct = 0.75f;

    public enum Type
    {
        Daggers,
        Gloves,
        Greatsword,
        Katana,
        Spear,
        Sword
    };


    private void Awake()
    {

        switch (archetype)
        {
            case Type.Daggers:
                uniqueAbility = gameObject.AddComponent<UniqueDaggers>();
                break;
            case Type.Gloves:
                uniqueAbility = gameObject.AddComponent<UniqueGloves>();
                break;
            case Type.Greatsword:
                uniqueAbility = gameObject.AddComponent<UniqueGreatsword>();
                break;
            case Type.Katana:
                uniqueAbility = gameObject.AddComponent<UniqueKatana>();
                break;
            case Type.Spear:
                uniqueAbility = gameObject.AddComponent<UniqueSpear>();
                break;
            case Type.Sword:
                uniqueAbility = gameObject.AddComponent<UniqueSword>();
                break;
        }
    }

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
