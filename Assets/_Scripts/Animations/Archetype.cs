using UnityEngine;

public class Archetype
{
    [SerializeField] private AttackInput[] attacks;
    [SerializeField] private AttackInput unique;
    [SerializeField] private AttackInput block;
    [SerializeField] private AttackInput[] parry;


    private void Awake()
    {
        
    }
}
