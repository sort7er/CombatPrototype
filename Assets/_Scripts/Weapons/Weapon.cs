using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Archetype archetype;
    public AttackInput closeAbilityInput;
    public AttackInput throwAbilityInput;
    public Transform[] weaponModels;


    public Attack closeAbility;
    public Attack throwAbility;

    private void Awake()
    {
        archetype.SetUpAnimations();
        //SetUpAttack(ref closeAbility, closeAbilityInput);
        //SetUpAttack(ref throwAbility, throwAbilityInput);
    }
    public void SetUpAttack(ref Attack attacksToSetUp, AttackInput inputs)
    {
        attacksToSetUp = new Attack(inputs.animationClip, inputs.activeWield);
    }
    public void SetParent(Transform[] parents)
    {
        for(int i = 0; i < weaponModels.Length; i++)
        {
            weaponModels[i].parent = parents[i];
            weaponModels[i].localPosition= Vector3.zero;
            weaponModels[i].localRotation= Quaternion.identity;
        }
    }
}
