using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Archetype archetype;

    public AttackInput closeAbilityInput;
    public AttackInput throwAbilityInput;

    public Attack closeAbility;
    public Attack throwAbility;

    private void Awake()
    {
        archetype.SetUpAnimations();
        SetUpAttack(ref closeAbility, closeAbilityInput);
        SetUpAttack(ref throwAbility, throwAbilityInput);
    }
    public void SetUpAttack(ref Attack attacksToSetUp, AttackInput inputs)
    {
        attacksToSetUp = new Attack(inputs.animationClip, inputs.activeWield);
    }
}
