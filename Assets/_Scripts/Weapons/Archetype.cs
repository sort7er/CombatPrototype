using UnityEngine;

[CreateAssetMenu(fileName = "New Archetype", menuName = "Archetype")]
public class Archetype: ScriptableObject
{
    [SerializeField] private AnimationInput idleInput;
    [SerializeField] private AttackInput[] attacksInput;
    [SerializeField] private AttackInput uniqueInput;
    [SerializeField] private AttackInput blockInput;
    [SerializeField] private AttackInput[] parryInput;


    public Anim idle;
    public Attack[] attacks;
    public Attack unique;
    public Attack block;
    public Attack[] parry;

    public void SetUpAnimations()
    {
        idle = new Anim(idleInput.animationClip);

        SetUpAttacks(ref attacks, attacksInput);
        SetUpAttacks(ref parry, parryInput);
        SetUpAttack(ref unique, uniqueInput);
        SetUpAttack(ref block, blockInput);
    }

    public void SetUpAttack(ref Attack attacksToSetUp, AttackInput inputs)
    {
        attacksToSetUp = new Attack(inputs.animationClip, inputs.activeWield);
    }

    public void SetUpAttacks(ref Attack[] attacksToSetUp, AttackInput[] inputs)
    {
        attacksToSetUp = new Attack[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            SetUpAttack(ref attacksToSetUp[i], inputs[i]);
        }
    }
}