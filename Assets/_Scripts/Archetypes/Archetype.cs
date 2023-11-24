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

    private UniqueDaggers uniqueDaggers = new UniqueDaggers();
    private UniqueGloves uniqueGloves = new UniqueGloves();
    private UniqueGreatsword uniqueGreatsword = new UniqueGreatsword();
    private UniqueKatana uniqueKatana = new UniqueKatana();
    private UniqueSpear uniqueSpear = new UniqueSpear();
    private UniqueSword uniqueSword = new UniqueSword();

    private void Awake()
    {
        switch(archetype)
        {
            case Type.Daggers:
                uniqueAbility = uniqueDaggers;
                break;
            case Type.Gloves:
                uniqueAbility = uniqueGloves;
                break;
            case Type.Greatsword:
                uniqueAbility = uniqueGreatsword;
                break;
            case Type.Katana:
                uniqueAbility = uniqueKatana;
                break;
            case Type.Spear:
                uniqueAbility = uniqueSpear;
                break;
            case Type.Sword:
                uniqueAbility = uniqueSword;
                break;
        }
    }

}
