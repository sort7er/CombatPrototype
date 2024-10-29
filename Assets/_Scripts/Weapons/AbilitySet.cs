using UnityEngine;

[CreateAssetMenu(fileName = "New Ability Set", menuName = "Ability Set")]
public class AbilitySet : ScriptableObject
{
    [Header("Player")]
    [SerializeField] private AttackInput meleeInput;
    [SerializeField] private AttackInput rangedInput;
    [Header("Enemy")]
    [SerializeField] private AttackEnemyInput enemyMeleeInput;
    [SerializeField] private AttackEnemyInput enemyRangedInput;

    public Attack melee;
    public Attack ranged;

    public AttackEnemy enemyMelee;
    public AttackEnemy enemyRanged;

    public void SetUpAnimations()
    {
        melee = Tools.SetUpAttack(meleeInput);
        ranged = Tools.SetUpAttack(rangedInput);

        enemyMelee = Tools.SetUpEnemyAttack(enemyMeleeInput);
        enemyRanged = Tools.SetUpEnemyAttack(enemyRangedInput);
    }

}
