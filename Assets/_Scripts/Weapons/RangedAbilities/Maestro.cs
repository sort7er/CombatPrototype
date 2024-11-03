using EnemyAI;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Ability
{
    //References
    private AnimationCurve animationCurve;

    //Targets
    private Vector3[] targetPos;
    private Quaternion[] targetRotation;


    //Constant positions
    private Vector3 centerPos;
    private Vector3 leftCorner;
    private Vector3 rightCorner;
    private float distanceFromTarget = 8;

    //Sequence
    private float timeInHand = 0.5f;
    private float timeToFlyBack = 0.5f;
    private float timeFlying;
    private float flyingInterval;
    private float timeElapsed;
    private float duration;

    // Values
    private float interpolation = 8;
    private bool backToHands;
    private bool left;


    public override void InitializeAbility()
    {
        base.InitializeAbility();
        range = distanceFromTarget * 2;
        targetPos = new Vector3[2];
        targetRotation = new Quaternion[2];
    }

    public override void ExecuteAbility(Player player, List<Enemy> enemies)
    {
        base.ExecuteAbility(player, enemies);    
        Common();

    }
    public override void ExecuteAbilityNoTarget(Player player)
    {
        base.ExecuteAbilityNoTarget(player);
        Common();
    }


    private void Common()
    {
        ReleaseCurrentWeapon();
        SetValues();
        OneLeft();
    }
    private void SetValues()
    {
        animationCurve = player.currentWeapon.currentAttack.animationCurve;
        backToHands = false;
        timeFlying = player.currentWeapon.abilitySet.ranged.duration - timeToFlyBack - timeInHand;
        flyingInterval = timeFlying / 3;
        timeElapsed = 1;
    }
    public override void LateUpdateAbility()
    {
        SettingTargetTransforms();
        MovingAbilityTransforms();
    }
    #region UpdateMethods
    private void SettingTargetTransforms()
    {
        if (!backToHands)
        {
            Quaternion baseRotation = Quaternion.LookRotation(-player.Up());
            targetRotation[0] = targetRotation[1] = Quaternion.Euler(baseRotation.eulerAngles.x, player.Rotation().eulerAngles.y, baseRotation.eulerAngles.z);


            centerPos = player.Position() + player.Forward() * distanceFromTarget;
            centerPos.y = player.cameraController.CameraPosition().y;
            leftCorner = centerPos - player.Right() * 5 + player.Up();
            rightCorner = centerPos + player.Right() * 5 + player.Up();

            DeterimeTarget();
        }
        else
        {
            for (int i = 0; i < abilityTransforms.Length; i++)
            {
                targetPos[i] = player.weaponTransform[i].position;
                targetRotation[i] = player.weaponTransform[i].rotation;
                
            }
        }
    }

    private void DeterimeTarget()
    {
        if (left)
        {
            targetPos[0] = targetPos[1] = leftCorner;
        }
        else
        {
            targetPos[0] = targetPos[1] = rightCorner;
        }
    }

    private void MovingAbilityTransforms()
    {
        for (int i = 0; i < abilityTransforms.Length; i++)
        {
            MoveAbilityTransform(abilityTransforms[i], targetPos[i], targetRotation[i]);
        }
    }
    private void MoveAbilityTransform(Transform abilityTrans, Vector3 targetPos, Quaternion targetRot)
    {
        if(timeElapsed < duration)
        {
            float t = timeElapsed / duration;

            Vector3 currentPos = Vector3.Lerp(abilityTrans.position, targetPos,t);

            float remapedTime = Tools.Remap(timeElapsed, 0, duration, 0, 1);
            float y = currentPos.y - animationCurve.Evaluate(remapedTime);
            Vector3 pos = new Vector3(currentPos.x, y, currentPos.z);
            abilityTrans.position = pos;

            abilityTrans.rotation = Quaternion.Slerp(abilityTrans.rotation, targetRot, t);
            timeElapsed += Time.deltaTime;


        }
        else
        {
            abilityTrans.position = targetPos;
            abilityTrans.rotation = targetRot;
        }

    }

    #endregion


    private void OneLeft()
    {
        left = true;
        timeElapsed = 0;
        duration = flyingInterval;
        player.InvokeMethod(TwoRight, flyingInterval);
    }
    private void TwoRight()
    {
        left = false;
        timeElapsed = 0;
        duration = flyingInterval;
        player.InvokeMethod(ThreeLeft, flyingInterval);
    }
    private void ThreeLeft()
    {
        timeElapsed = 0;
        duration = flyingInterval;
        left = true;
        player.InvokeMethod(FlyBack, flyingInterval);
    }


    private void FlyBack()
    {
        backToHands = true;
        timeElapsed = 0;
        duration = timeToFlyBack;
        player.InvokeMethod(Return, timeToFlyBack);
    }
    private void Return()
    {
        ReturnCurrentWeapon();
    }



    private void CastBox()
    {
        Collider[] hits;
        hits = Physics.OverlapBox(centerPos, new Vector3(3, 2, distanceFromTarget), player.Rotation(), 6);
    }
}
