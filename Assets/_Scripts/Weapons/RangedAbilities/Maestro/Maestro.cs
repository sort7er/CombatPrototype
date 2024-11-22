using EnemyAI;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Ability
{
    //Sequence
    public float duration { get; private set; }
    public bool backToHands { get; private set; }
    public bool isLeft { get; private set; }
    public bool isMiddleCurve { get; private set; }

    private List<Enemy> enemiesDamaged = new();
    private MaestroMovement maestroMovement;
    private float timeToFlyBack = 0.6f;
    private int numberOfSwings;

    //Damage
    private float damageRange = 3f; //should be generous
    private bool damagePhaseDone;

    public override void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        base.ExecuteAbility(player, enemies);
        SetValues();
        //player.handEffects.EnableRight();
    }
    public override void ExecuteAbilityNoTarget(Player player)
    {
        base.ExecuteAbilityNoTarget(player);
        SetValues();
    }
    private void SetValues()
    {
        if(maestroMovement == null)
        {
            maestroMovement = new MaestroMovement(this);
        }

        backToHands = false;
        isMiddleCurve = false;
        numberOfSwings = 0;
        
        duration = 0.75f;
        maestroMovement.SetStartValues();
        maestroMovement.SetStartTransforms();
        player.currentWeapon.weaponEffects.EnableGlow();
        abilityTransforms[0].GetComponent<AudioSource>().Play();
    }

    public override void AbilityPing()
    {
        if (numberOfSwings == 0)
        {
            OneLeft();
        }
        else if (numberOfSwings == 1)
        {
            TwoRight();
        }
        else if (numberOfSwings == 2)
        {
            ThreeLeft();
        }
        else
        {
            FlyBack();
        }

        //Cast box after methods to have startpositions updated
        maestroMovement.CastBox();
        numberOfSwings++;
    }
    #region Sequence


    private void OneLeft()
    {
        player.currentWeapon.weaponEffects.EnableTrails();
        ReleaseCurrentWeapon();
        SwingStartCommon();
        isLeft = true;
    }
    private void TwoRight()
    {
        SwingStartCommon();
        isMiddleCurve = true;
        isLeft = false;
    }
    private void ThreeLeft()
    {
        SwingStartCommon();
        isLeft = true;
    }

    private void FlyBack()
    {
        SwingStartCommon();
        backToHands = true;
        isMiddleCurve = false;
        duration = timeToFlyBack;
        player.InvokeMethod(Return, timeToFlyBack);
    }
    private void SwingStartCommon()
    {
        maestroMovement.ResetTimeElapsed();
        maestroMovement.SetStartTransforms();
        damagePhaseDone = false;
    }
    private void Return()
    {
        player.currentWeapon.weaponEffects.DisableTrails();
        player.currentWeapon.weaponEffects.DisableGlow();
        abilityTransforms[0].GetComponent<AudioSource>().Stop();
        //player.handEffects.DisableRight();
        ReturnCurrentWeapon();

    }
    #endregion

    public override void LateUpdateAbility()
    {
        maestroMovement.SettingTargetTransforms();
        maestroMovement.MovingAbilityTransforms();
    }
    public override void FixedUpdateAbility()
    {
        if(maestroMovement.timeElapsed < duration)
        {
            LookForEnemies();
        }
        else
        {
            if(!damagePhaseDone)
            {         
                ResetEnemyList();             
            }
        }
    }

    private void LookForEnemies()
    {
        enemies.Clear();
        enemies = player.targetAssistance.CastBox(maestroMovement.BoxCenter(), maestroMovement.BoxSize(), playerTrans.rotation);

        for(int i = 0; i < enemies.Count; i++)
        {
            if (!enemiesDamaged.Contains(enemies[i]))
            {
                CheckIfDamage(enemies[i]);
            }
        }

    }

    private void CheckIfDamage(Enemy enemy)
    {
        if (Vector3.Distance(enemy.Position(), abilityTransforms[0].position) < damageRange)
        {
            //Damage enemy
            enemy.HitJustCut(player.currentWeapon.currentAttack, player, abilityTransforms[0].position, -abilityTransforms[0].forward);

            enemiesDamaged.Add(enemy);
        }
    }

    private void ResetEnemyList()
    {
        damagePhaseDone = true;
        enemiesDamaged.Clear();
    }

}
