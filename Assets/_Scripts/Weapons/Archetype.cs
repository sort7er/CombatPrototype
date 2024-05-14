using Attacks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Archetype", menuName = "Archetype")]
public class Archetype: ScriptableObject
{
    public bool showStartPos;
    public bool showEndPos;
    public bool showEffect;

    public float effectSize = 1;
    public ParticleSystem slashEffect;

    [Header("Player")]
    [SerializeField] private AnimationInput idleInput;
    [SerializeField] private string walkInput;
    [SerializeField] private AnimationInput jumpInput;
    [SerializeField] private AnimationInput fallInput;
    [SerializeField] private AttackInput[] attacksInput;
    [SerializeField] private AttackInput uniqueInput;
    [SerializeField] private AttackInput blockInput;
    [SerializeField] private AttackInput[] parryInput;
    [SerializeField] private AttackInput[] perfectParryInput;
    [SerializeField] private AttackInput[] parryfollowUpAttackInput;


    [Header("Enemy")]
    [SerializeField] private AttackInput[] enemyAttacksInput;
    [SerializeField] private AttackInput[] enemyParrysInput;
    [SerializeField] private AnimationInput enemyStaggeredInput;
    [SerializeField] private AnimationInput enemyStunnedInput;

    public Anim idle;
    public Anim walk;
    public Anim jump;
    public Anim fall;
    public Attack[] attacks;
    public Attack unique;
    public Attack block;
    public Attack[] parry;
    public Attack[] perfectParry;
    public Attack[] parryfollowUpAttack;

    public Attack[] enemyAttacks;
    public Attack[] enemyParrys;
    public Anim enemyStaggered; 
    public Anim enemyStunned;

    public void SetUpAnimations()
    {
        idle = new Anim(idleInput.animationClip);
        walk = new Anim(walkInput);
        jump = new Anim(jumpInput.animationClip);
        fall = new Anim(fallInput.animationClip);

        SetUpAttacks(ref attacks, attacksInput);
        SetUpAttack(ref unique, uniqueInput);
        SetUpAttack(ref block, blockInput);
        SetUpAttacks(ref parry, parryInput);
        SetUpAttacks(ref perfectParry, perfectParryInput);
        SetUpAttacks(ref parryfollowUpAttack, parryfollowUpAttackInput);

        SetUpAttacks(ref enemyAttacks, enemyAttacksInput);
        SetUpAttacks(ref enemyParrys, enemyParrysInput);

        enemyStaggered = new Anim(enemyStaggeredInput.animationClip);
        enemyStunned = new Anim(enemyStunnedInput.animationClip);

    }

    public void SetUpAttack(ref Attack attack, AttackInput inputs)
    {

        bool right = inputs.activeWield == Wield.right || inputs.activeWield == Wield.both;
        bool left = inputs.activeWield == Wield.left || inputs.activeWield == Wield.both;

        if (right && inputs.attackCoordsMain.Length == 0)
        {
            inputs.attackCoordsMain = new AttackCoord[1];
            inputs.attackCoordsMain[0] = new AttackCoord(Vector3.zero, Vector3.zero);
        }

        if (left && inputs.attackCoordsSecondary.Length == 0)
        {
            inputs.attackCoordsSecondary = new AttackCoord[1];
            inputs.attackCoordsSecondary[0] = new AttackCoord(Vector3.zero, Vector3.zero);
        }

        attack = new Attack(inputs.animationClip, inputs.damage, inputs.postureDamage, inputs.exitTime, inputs.transitionDuration, inputs.activeWield, inputs.hitType, inputs.attackCoordsMain, inputs.attackCoordsSecondary);
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