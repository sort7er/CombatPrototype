using UnityEngine;
using Attacks;
using DynamicMeshCutter;

public class Weapon : MonoBehaviour
{
    [Header("Temporary damage")]
    public int damage;
    public int postureDamage;
    public float pushbackForce;

    [Header("Attacks")]
    public Archetype archetype;
    public UniqueAbility uniqueAbility;
    //[SerializeField] private AttackInput closeAbilityInput;
    //[SerializeField] private AttackInput throwAbilityInput;

    [Header("Weapons")]
    [SerializeField] private WeaponModel[] weaponModel;
    public Humanoid owner { get; private set; }
    public Attack currentAttack { get; private set; }

    private Attack closeAbility;
    private Attack throwAbility;
    
    private SlicingWeapon[] slicingWeapons;
    private bool sliceEnded;

    private int rightEffect;
    private int leftEffect;

    private bool rightIncluded;
    private bool leftIncluded;


    private void Awake()
    {
        archetype.SetUpAnimations();
        SetUpSlicingWeapons();

        //SetUpAttack(ref closeAbility, closeAbilityInput);
        //SetUpAttack(ref throwAbility, throwAbilityInput);
    }
    public void SetUpAttack(ref Attack attacksToSetUp, AttackInput inputs)
    {
        attacksToSetUp = new Attack(inputs.animationClip, inputs.activeWield, inputs.hitType, inputs.attackCoordsMain, inputs.attackCoordsSecondary);
    }

    //Set from player actions
    public void SetOwner(Humanoid owner, Transform weaponParent, Transform[] modelParents)
    {
        this.owner = owner;
        transform.parent = weaponParent;
        transform.localRotation= Quaternion.identity;
        transform.localPosition= Vector3.zero;

        for (int i = 0; i < weaponModel.Length; i++)
        {
            weaponModel[i].SetParent(modelParents[i]);
        }
    }
    public void NoAttack()
    {
        currentAttack = null;
    }
    public void Attack(Attack newAttack)
    {
        currentAttack = newAttack;

        leftEffect= 0;
        rightEffect= 0;

        rightIncluded = currentAttack.currentWield == Wield.right || currentAttack.currentWield == Wield.both;
        leftIncluded = currentAttack.currentWield == Wield.left || currentAttack.currentWield == Wield.both;

    }
    public void Effect()
    {
        if (rightIncluded)
        {
            weaponModel[0].Effect(currentAttack.attackCoordsMain[rightEffect]);
            rightEffect++;
        }
        
        if (leftIncluded)
        {
            if(currentAttack.attackCoordsSecondary == null)
            {
                Debug.Log(currentAttack.animationClip.name);
            }
            weaponModel[1].Effect(currentAttack.attackCoordsSecondary[leftEffect]);
            leftEffect++;
        }
    }

    public void AttackDone()
    {
        if(rightIncluded)
        {
            weaponModel[0].AttackDone();
        }
        if(leftIncluded)
        {
            weaponModel[1].AttackDone();
        }
    }
    public void Hit(Vector3 hitPoint)
    {
        if (rightIncluded)
        {
            weaponModel[0].Hit(hitPoint);
        }

        if (leftIncluded)
        {
            weaponModel[1].Hit(hitPoint);
        }
    }

    #region Slice related methods
    public void Slice(MeshTarget mesh)
    {
        if(currentAttack.hitType == HitType.slice && slicingWeapons[0] != null)
        {
            sliceEnded = false;
            if (currentAttack.currentWield == Wield.right)
            {
                slicingWeapons[0].Slice(mesh);
            }
            else if (currentAttack.currentWield == Wield.left)
            {
                slicingWeapons[1].Slice(mesh);
            }
            else
            {
                slicingWeapons[0].OnMeshCreated += DelayedSlice;
                slicingWeapons[0].Slice(mesh);
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
    private void DelayedSlice(MeshCreationData data)
    {
        slicingWeapons[0].OnMeshCreated -= DelayedSlice;

        if (!sliceEnded)
        {
            sliceEnded = true;
            for(int i = 0; i < data.CreatedTargets.Length; i++)
            {
                slicingWeapons[1].Slice(data.CreatedTargets[i]);
            }
        }
    }

    public void Vissible()
    {
        for (int i = 0; i < weaponModel.Length; i++)
        {
            weaponModel[i].gameObject.SetActive(true);
        }
    }
    public void Hidden()
    {
        for (int i = 0; i < weaponModel.Length; i++)
        {
            weaponModel[i].gameObject.SetActive(false);
        }
    }

    #endregion
}
