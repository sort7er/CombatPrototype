using UnityEngine;

public class EnemyHealth : Health
{
    private Enemy enemy;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
    }
    public override void TakeDamage(int damage, DamageType incomingDamage)
    {
        base.TakeDamage(damage);
        enemy.Hit();
    }
    protected override void Dead(DamageType incomingDamage)
    {
        base.Dead(incomingDamage);
        Debug.Log(incomingDamage.ToString());

    }
}
