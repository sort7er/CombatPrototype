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
    [SerializeField] private AnimationInput enemyBlockInput;
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
    public Anim enemyBlock; 
    public Anim enemyStaggered; 
    public Anim enemyStunned;
    public Anim enemyHit;
    public Anim enemyStandby;
    public Anim enemyStandbyTurnLeft;
    public Anim enemyStandbyTurnRight;

    public void SetUpAnimations()
    {
        //For player
        idle = new Anim(idleInput.animationClip);
        walk = new Anim(walkInput.animationClip);
        jump = new Anim(jumpInput.animationClip);
        fall = new Anim(fallInput.animationClip);
        staggered = new Anim(staggeredInput.animationClip);
        hit = new Anim(hitInput.animationClip);
        stunned = new Anim(stunnedInput.animationClip);

        SetUpAttacks(ref attacks, attacksInput);
        SetUpAttack(ref unique, uniqueInput);
        SetUpAttack(ref block, blockInput);
        SetUpAttacks(ref parry, parryInput);
        SetUpAttacks(ref perfectParry, perfectParryInput);
        SetUpAttacks(ref parryAttack, parryAttackInput);

        //For enemy
        enemyBlock = new Anim(enemyBlockInput.animationClip);
        enemyStaggered = new Anim(enemyStaggeredInput.animationClip);
        enemyStunned = new Anim(enemyStunnedInput.animationClip);
        enemyHit = new Anim(enemyHitInput.animationClip);
        enemyStandby = new Anim(enemyStandbyInput.animationClip);
        enemyStandbyTurnLeft = new Anim(enemyStandbyTurnLeftInput.animationClip);
        enemyStandbyTurnRight = new Anim(enemyStandbyTurnRightInput.animationClip);

        SetUpEnemyAttacks(ref enemyAttacks, enemyAttacksInput);
        SetUpEnemyAttacks(ref enemyParrys, enemyParrysInput);
        SetUpEnemyAttack(ref enemyPerfectParry, enemyPerfectParryInput);
        SetUpEnemyAttack(ref enemyParryAttack, enemyParryAttackInput);
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

        attack = new Attack(inputs.animationClip, inputs.damage, inputs.postureDamage, inputs.activeWield, inputs.hitType, inputs.attackCoordsMain, inputs.attackCoordsSecondary);
    }

    public void SetUpAttacks(ref Attack[] attacksToSetUp, AttackInput[] inputs)
    {
        attacksToSetUp = new Attack[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            SetUpAttack(ref attacksToSetUp[i], inputs[i]);
        }
    }

    public void SetUpEnemyAttack(ref AttackEnemy attack, AttackEnemyInput inputs)
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

        attack = new AttackEnemy(inputs.animationClip, inputs.damage, inputs.postureDamage, inputs.activeWield, inputs.hitType, inputs.attackCoordsMain, inputs.attackCoordsSecondary, inputs.exitTime, inputs.transitionDuration);
    }
    public void SetUpEnemyAttacks(ref AttackEnemy[] enemyAttacksToSetUp, AttackEnemyInput[] inputs)
    {
        enemyAttacksToSetUp = new AttackEnemy[inputs.Length];

        for (int i = 0; i < inputs.Length; i++)
        {
            SetUpEnemyAttack(ref enemyAttacksToSetUp[i], inputs[i]);
        }
    }
}