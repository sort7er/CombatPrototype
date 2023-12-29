using UnityEngine;
using HealthRelated;

public class EnemyHealth : Health
{
    [SerializeField] private SlicableObject[] meshes;

    private Enemy enemy;
    

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
    }
    public override void TakeDamage(int damage, DamageType incomingDamage)
    {
        base.TakeDamage(damage, incomingDamage);
        Debug.Log("Hit " + incomingDamage.ToString());
        enemy.Hit();
    }
    protected override void Dead(DamageType incomingDamage)
    {
        base.Dead(incomingDamage);
        Debug.Log("Dead " + incomingDamage.ToString());

        if(incomingDamage == DamageType.Slice)
        {
            for (int i = 0; i < meshes.Length; i++) 
            {
                enemy.player.weaponSelector.currentArchetype.currentSlicingObject.Slice(meshes[i]);
            }
        }

    }
}
