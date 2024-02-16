using UnityEngine;
using Attacks;
using System;
using System.Collections.Generic;

public class Weapon : MonoBehaviour
{
    [Header("Temporary damage")]
    public int damage;
    public int postureDamage;

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

    private List<WeaponModel> weaponModelList = new();
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
        attacksToSetUp = new Attack(inputs.animationClip, inputs.activeWield, inputs.hitType, inputs.attackCoords);
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
        SetCurrentWeaponModels();
        for (int i = 0; i < weaponModelList.Count; i++)
        {
            weaponModelList[i].Attack(currentAttack.attackCoords[i]);
        }
    }
    public void AttackDone()
    {
        for (int i = 0; i < weaponModel.Length; i++)
        {
            weaponModel[i].AttackDone();
        }
    }

    #region Slice related methods
    public void Slice(SlicableMesh mesh)
    {
        if(currentAttack.hitType == HitType.slice && slicingWeapons[0] != null)
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
    public void Hit(Vector3 hitPoint)
    {
        SetCurrentWeaponModels();

        for (int i = 0; i < weaponModelList.Count; i++)
        {
            weaponModelList[i].Hit(hitPoint);
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



    private void SetCurrentWeaponModels()
    {
        weaponModelList.Clear();

        if (currentAttack.currentWield == Wield.right)
        {
            weaponModelList.Add(weaponModel[0]);
        }
        else if (currentAttack.currentWield == Wield.left)
        {
            weaponModelList.Add(weaponModel[1]);
        }
        else
        {
            weaponModelList.Add(weaponModel[0]);
            weaponModelList.Add(weaponModel[1]);
        }
    }
}
