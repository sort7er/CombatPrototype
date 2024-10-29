using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Set", menuName = "Ability Set")]
public class AbilitySet : ScriptableObject
{
    public enum Melee
    {
        None
    }
    public enum Ranged
    {
        Maestro,
        None
    }


    public Melee meleeType;
    public Ranged rangedType;

    [SerializeField] private AttackInput meleeInput;
    [SerializeField] private AttackInput rangedInput;


    public Attack melee;
    public Attack ranged;

    public Ability meleeAbility;
    public Ability rangedAbilty;

    public void SetUpAnimations()
    {
        melee = Tools.SetUpAttack(meleeInput);
        ranged = Tools.SetUpAttack(rangedInput);

        SetRanged();
    }

    private void SetRanged()
    {
        if (rangedType == Ranged.Maestro)
        {
            rangedAbilty = new Maestro();
        }

        rangedAbilty.SetParamaters();
    }
    //private void SetMelee()
    //{
    //    if (meleeType == Melee.None)
    //    {
    //        meleeAbility = new UniqueBrawling();
    //    }

    //    rangedAbilty.SetParamaters();
    //}

}
