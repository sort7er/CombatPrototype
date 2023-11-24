using UnityEngine;

public class Archetype : MonoBehaviour
{


    public string archetypeName;
    public ArchetypeAnimator archetypeAnimator;
    public UniqueAbility uniqueAbility;
    [SerializeField] private Type archetype;

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

}
