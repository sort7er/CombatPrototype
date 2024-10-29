using Attacks;
using UnityEngine;

[CreateAssetMenu(fileName = "New Archetype", menuName = "Archetype")]
public class Archetype: ScriptableObject
{

    public enum Type
    {
        Brawling,
        Daggers,
        Greatsword,
        Katana,
        Spear,
        Sword
    }

    public bool showStartPos;
    public bool showEndPos;
    public bool showEffect;

    public float effectSize = 1;
    public ParticleSystem slashEffect;

    public Type archetype;
    public UniqueAbility uniqueAbility;

    [Header("Player")]
    [SerializeField] private AnimationInput idleInput;
    [SerializeField] private AnimationInput walkInput;
    [SerializeField] private AnimationInput jumpInput;
    [SerializeField] private AnimationInput fallInput;
    [SerializeField] private AnimationInput staggeredInput;
    [SerializeField] private AnimationInput hitInput;
    [SerializeField] private AnimationInput stunnedInput;
    [SerializeField] private AttackInput[] attacksInput;
    [SerializeField] private AttackInput uniqueInput;
    [SerializeField] private AttackInput blockInput;
    [SerializeField] private AttackInput[] parryInput;
    [SerializeField] private AttackInput[] perfectParryInput;
    [SerializeField] private AttackInput[] parryAttackInput;


    [Header("Enemy")]
    [SerializeField] private AttackEnemyInput[] enemyAttacksInput;
    [SerializeField] private AttackEnemyInput[] enemyParrysInput;
    [SerializeField] private AttackEnemyInput enemyPerfectParryInput;
    [SerializeField] private AttackEnemyInput enemyParryAttackInput;
    [SerializeField] private AttackEnemyInput enemyBlockInput;
    [SerializeField] private AnimationInput enemyStaggeredInput;
    [SerializeField] private AnimationInput enemyStunnedInput;
    [SerializeField] private AnimationInput enemyHitInput;
    [SerializeField] private AnimationInput enemyStandbyInput;
    [SerializeField] private AnimationInput enemyStandbyTurnLeftInput;
    [SerializeField] private AnimationInput enemyStandbyTurnRightInput;

    public Anim idle;
    public Anim walk;
    public Anim jump;
    public Anim fall;
    public Anim staggered;
    public Anim hit;
    public Anim stunned;
    public Attack[] attacks;
    public Attack unique;
    public Attack block;
    public Attack[] parry;
    public Attack[] perfectParry;
    public Attack[] parryAttack;

    public AttackEnemy[] enemyAttacks;
    public AttackEnemy[] enemyParrys;
    public AttackEnemy enemyPerfectParry;
    public AttackEnemy enemyParryAttack;
    public AttackEnemy enemyBlock; 
    public Anim enemyStaggered; 
    public Anim enemyStunned;
    public Anim enemyHit;
    public Anim enemyStandby;
    public Anim enemyStandbyTurnLeft;
    public Anim enemyStandbyTurnRight;

    public void SetUp()
    {
        SetUnique();

        //For player
        idle = new Anim(idleInput.animationClip);
        walk = new Anim(walkInput.animationClip);
        jump = new Anim(jumpInput.animationClip);
        fall = new Anim(fallInput.animationClip);
        staggered = new Anim(staggeredInput.animationClip);
        hit = new Anim(hitInput.animationClip);
        stunned = new Anim(stunnedInput.animationClip);

        unique = Tools.SetUpAttack(uniqueInput);
        block = Tools.SetUpAttack(blockInput);
        SetUpAttacks(ref attacks, attacksInput);
        SetUpAttacks(ref parry, parryInput);
        SetUpAttacks(ref perfectParry, perfectParryInput);
        SetUpAttacks(ref parryAttack, parryAttackInput);

        //For enemy
        enemyStaggered = new Anim(enemyStaggeredInput.animationClip);
        enemyStunned = new Anim(enemyStunnedInput.animationClip);
        enemyHit = new Anim(enemyHitInput.animationClip);
        enemyStandby = new Anim(enemyStandbyInput.animationClip);
        enemyStandbyTurnLeft = new Anim(enemyStandbyTurnLeftInput.animationClip);
        enemyStandbyTurnRight = new Anim(enemyStandbyTurnRightInput.animationClip);

        SetUpEnemyAttacks(ref enemyAttacks, enemyAttacksInput);
        SetUpEnemyAttacks(ref enemyParrys, enemyParrysInput);
        enemyBlock = Tools.SetUpEnemyAttack(enemyBlockInput);
        enemyParryAttack = Tools.SetUpEnemyAttack(enemyParryAttackInput);
        enemyPerfectParry = Tools.SetUpEnemyAttack(enemyPerfectParryInput);
    }

    public void SetUpAttacks(ref Attack[] attacksToSetUp, AttackInput[] inputs)
    {
        attacksToSetUp = new Attack[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            attacksToSetUp[i] = Tools.SetUpAttack(inputs[i]);
        }
    }

    public void SetUpEnemyAttacks(ref AttackEnemy[] enemyAttacksToSetUp, AttackEnemyInput[] inputs)
    {
        enemyAttacksToSetUp = new AttackEnemy[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            enemyAttacksToSetUp[i] = Tools.SetUpEnemyAttack(inputs[i]);
        }
    }

    private void SetUnique()
    {
        if(archetype == Type.Brawling)
        {
            uniqueAbility = new UniqueBrawling();
        }
        else if (archetype == Type.Daggers)
        {
            uniqueAbility = new UniqueDaggers();
        }
        else if (archetype == Type.Greatsword)
        {
            uniqueAbility = new UniqueGreatsword();
        }
        else if (archetype == Type.Katana)
        {
            uniqueAbility = new UniqueKatana();
        }
        else if (archetype == Type.Spear)
        {
            uniqueAbility = new UniqueSpear();
        }
        else if (archetype == Type.Sword)
        {
            uniqueAbility = new UniqueSword();
        }
        uniqueAbility.SetParamaters();
    }
}