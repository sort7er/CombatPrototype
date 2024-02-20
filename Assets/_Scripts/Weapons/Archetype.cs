using Attacks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Archetype", menuName = "Archetype")]
public class Archetype: ScriptableObject
{
    public float effectSize = 1;
    [SerializeField] private AnimationInput idleInput;
    [SerializeField] private AnimationInput walkInput;
    [SerializeField] private AnimationInput jumpInput;
    [SerializeField] private AnimationInput fallInput;
    [SerializeField] private AttackInput[] attacksInput;
    [SerializeField] private AttackInput uniqueInput;
    [SerializeField] private AttackInput blockInput;
    [SerializeField] private AttackInput[] parryInput;

    public Anim idle;
    public Anim walk;
    public Anim jump;
    public Anim fall;
    public Attack[] attacks;
    public Attack unique;
    public Attack block;
    public Attack[] parry;

    public void SetUpAnimations()
    {
        idle = new Anim(idleInput.animationClip);
        walk = new Anim(walkInput.animationClip);
        jump = new Anim(jumpInput.animationClip);
        fall = new Anim(fallInput.animationClip);

        SetUpAttacks(ref attacks, attacksInput);
        SetUpAttacks(ref parry, parryInput);
        SetUpAttack(ref unique, uniqueInput);
        SetUpAttack(ref block, blockInput);
    }

    public void SetUpAttack(ref Attack attack, AttackInput inputs)
    {
        if (inputs.attackCoords.Length <= 1 && inputs.activeWield == Wield.both)
        {
            inputs.attackCoords = new AttackCoord[2];
            inputs.attackCoords[0] = new AttackCoord(Vector3.zero, Vector3.zero);
            inputs.attackCoords[1] = new AttackCoord(Vector3.zero, Vector3.zero);
        }
        else if(inputs.attackCoords.Length == 0)
        {
            inputs.attackCoords = new AttackCoord[1];
            inputs.attackCoords[0] = new AttackCoord(Vector3.zero, Vector3.zero);
        }

        attack = new Attack(inputs.animationClip, inputs.activeWield, inputs.hitType, inputs.attackCoords);
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