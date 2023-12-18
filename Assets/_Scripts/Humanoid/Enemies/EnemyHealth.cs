using UnityEngine;

public class EnemyHealth : Health
{
    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }
    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);
        enemy.Hit();
    }
}
