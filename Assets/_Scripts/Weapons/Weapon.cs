using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Archetype archetype;
    public AttackInput closeAbilityInput;
    public AttackInput throwAbilityInput;

    [Header("References")]
    public WeaponModel[] weaponModel;

    //private Anim curAnim;
    public Humanoid owner { get; private set; }
    private Player player;
    private HitBox hitBox;

    private Attack curAttack;

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
        attacksToSetUp = new Attack(inputs.animationClip, inputs.activeWield);
    }

    //Set from player actions
    public void SetOwner(Humanoid owner, Transform[] parents)
    {
        this.owner = owner;
        if(owner is Player player)
        {
            this.player = player;
            hitBox = player.hitBox;
        }

        for (int i = 0; i < weaponModel.Length; i++)
        {
            weaponModel[i].SetParent(parents[i]);
        }
    }
    public void SetAttack(Attack newAttack)
    {
        curAttack = newAttack;
    }

    #region Slice related methods
    public void Slice(SlicableMesh mesh)
    {
        if(curAttack != null && slicingWeapons[0] != null)
        {
            sliceEnded = false;
            if (curAttack.currentWield == Attacks.Wield.right)
            {
                slicingWeapons[0].CheckSlice(mesh, hitBox);
            }
            else if (curAttack.currentWield == Attacks.Wield.left)
            {
                slicingWeapons[1].CheckSlice(mesh, hitBox);
            }
            else
            {
                slicingWeapons[0].OnSliceDone += DelayedSlice;
                slicingWeapons[0].CheckSlice(mesh, hitBox);
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
            slicingWeapons[1].CheckSlice(mesh1, hitBox);
            slicingWeapons[1].CheckSlice(mesh2, hitBox);
            sliceEnded = true;
        }
    }
    #endregion
}
