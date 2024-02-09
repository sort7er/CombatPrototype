using UnityEngine;
using Attacks;

public class Weapon : MonoBehaviour
{
    [Header("Temporary damage")]
    public int damage;
    public int postureDamage;

    [Header("Attacks")]
    public Archetype archetype;
    [SerializeField] private AttackInput closeAbilityInput;
    [SerializeField] private AttackInput throwAbilityInput;

    [Header("References")]
    [SerializeField] private WeaponModel[] weaponModel;
    public Humanoid owner { get; private set; }
    public Attack currentAttack { get; private set; }

    private Attack closeAbility;
    private Attack throwAbility;

    private SlicingWeapon[] slicingWeapons;
    private bool sliceEnded;


    private void Awake()
    {
        archetype.SetUpAnimations();
        SetUpSlicingWeapons();

        //SetUpAttack(ref closeAbility, closeAbilityInput);
        //SetUpAttack(ref throwAbility, throwAbilityInput);
    }
    public void SetUpAttack(ref Attack attacksToSetUp, AttackInput inputs)
    {
        attacksToSetUp = new Attack(inputs.animationClip, inputs.activeWield, inputs.hitType);
    }

    //Set from player actions
    public void SetOwner(Humanoid owner, Transform[] parents)
    {
        this.owner = owner;

        for (int i = 0; i < weaponModel.Length; i++)
        {
            weaponModel[i].SetParent(parents[i]);
        }
    }
    public void SetAttack(Attack newAttack)
    {
        currentAttack = newAttack;
    }
    public void AttackDone()
    {
        for (int i = 0; i < slicingWeapons.Length; i++)
        {
            weaponModel[i].SwingDone();
        }
    }

    #region Slice related methods
    public void Slice(SlicableMesh mesh)
    {
        if(currentAttack != null && currentAttack.hitType == HitType.slice && slicingWeapons[0] != null)
        {
            sliceEnded = false;
            if (currentAttack.currentWield == Wield.right)
            {
                slicingWeapons[0].CheckSlice(mesh);
            }
            else if (currentAttack.currentWield == Wield.left)
            {
                slicingWeapons[1].CheckSlice(mesh);
            }
            else
            {
                slicingWeapons[0].OnSliceDone += DelayedSlice;
                slicingWeapons[0].CheckSlice(mesh);
            }
        }
    }

    private void SetUpSlicingWeapons()
    {
        slicingWeapons = new SlicingWeapon[weaponModel.Length];
        for (int i = 0; i < slicingWeapons.Length; i++)
        {
            if (weaponModel[i] is SlicingWeapon slice)
            {
                slicingWeapons[i] = slice;
            }
        }
    }
    private void DelayedSlice(SlicableMesh mesh1, SlicableMesh mesh2)
    {
        slicingWeapons[0].OnSliceDone -= DelayedSlice;
        if (!sliceEnded)
        {
            slicingWeapons[1].CheckSlice(mesh1);
            slicingWeapons[1].CheckSlice(mesh2);
            sliceEnded = true;
        }
    }
    #endregion
}
