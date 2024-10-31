using DG.Tweening;
using EnemyAI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Ability
{

    private AnimationCurve animationCurve;
    private Vector3[] targetPos;
    private Quaternion[] targetRotation;
    private Vector3 centerPos;
    private Vector3 leftCorner;
    private Vector3 rightCorner;
    private float distanceFromTarget = 8;
    private float interpolation = 8;
    private float timeToFlyBack = 0.5f;
    private float timeFlying;

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
        animationCurve = player.currentWeapon.currentAttack.animationCurve;
        backToHands = false;
        timeFlying = player.currentWeapon.abilitySet.ranged.duration - timeToFlyBack;

        player.InvokeMethod(FlyBack, timeFlying);
        player.InvokeMethod(MaestroDone, player.currentWeapon.abilitySet.ranged.duration);
        OneLeft();

    }

    public override void UpdateAbility()
    {
        if(!backToHands)
        {
            centerPos = player.Position() + player.Forward() * distanceFromTarget;
            centerPos.y = player.cameraController.CameraPosition().y;
            leftCorner = centerPos - player.Right() * 5 + player.Up();
            rightCorner = centerPos + player.Right() * 5 + player.Up();
            
            if(left)
            {
                targetPos[0] = leftCorner;
                targetPos[1] = leftCorner;
            }
            else
            {
                targetPos[0] = rightCorner;
                targetPos[1] = rightCorner;
            }

            Quaternion baseRotation = Quaternion.LookRotation(-player.Up());
            targetRotation[0] = targetRotation[1] = Quaternion.Euler(baseRotation.eulerAngles.x, player.Rotation().eulerAngles.y, baseRotation.eulerAngles.z);
        }
        else
        {
            for (int i = 0; i < abilityTransforms.Length; i++)
            {
                targetPos[i] = player.weaponTransform[i].position;
                targetRotation[i] = player.weaponTransform[i].rotation;
            }
        }

        for (int i = 0; i < abilityTransforms.Length; i++)
        {
            abilityTransforms[i].position = Vector3.Lerp(abilityTransforms[i].position, targetPos[i], interpolation * Time.deltaTime);
            abilityTransforms[i].rotation = Quaternion.Slerp(abilityTransforms[i].rotation, targetRotation[i], interpolation * Time.deltaTime);
        }

    }

    private void OneLeft()
    {
        left = true;
        player.InvokeMethod(TwoRight, timeFlying * 0.333f);
    }
    private void TwoRight()
    {
        left = false;
        player.InvokeMethod(ThreeLeft, timeFlying * 0.333f);
    }
    private void ThreeLeft()
    {
        left = true;
    }
    private void CastBox()
    {
        Collider[] hits;
        hits = Physics.OverlapBox(centerPos, new Vector3(3, 2, distanceFromTarget), player.Rotation(), 6);
    }

    private void FlyBack()
    {
        backToHands = true;
    }
    private void MaestroDone()
    {
        ReturnCurrentWeapon();
    }
}
