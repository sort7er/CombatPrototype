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

    private MaestroMovement maestroMovement;
    private float timeToFlyBack = 0.6f;
    private int numberOfSwings;

    public override void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        base.ExecuteAbility(player, enemies);
        SetValues();

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
        
        duration = 0.7f;
        maestroMovement.SetStartValues();
        maestroMovement.SetStartTransforms();
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
        ReleaseCurrentWeapon();
        maestroMovement.SetStartTransforms();
        maestroMovement.ResetTimeElapsed();
        isLeft = true;
    }
    private void TwoRight()
    {
        maestroMovement.SetStartTransforms();
        maestroMovement.ResetTimeElapsed();
        isMiddleCurve = true;
        isLeft = false;
    }
    private void ThreeLeft()
    {
        maestroMovement.SetStartTransforms();
        maestroMovement.ResetTimeElapsed();
        isLeft = true;
    }

    private void FlyBack()
    {
        maestroMovement.ResetTimeElapsed();
        maestroMovement.SetStartTransforms();
        backToHands = true;
        isMiddleCurve = false;
        duration = timeToFlyBack;
        player.InvokeMethod(Return, timeToFlyBack);
    }
    private void Return()
    {
        ReturnCurrentWeapon();
    }
    #endregion

    public override void LateUpdateAbility()
    {
        maestroMovement.SettingTargetTransforms();
        maestroMovement.MovingAbilityTransforms();
    }


}
