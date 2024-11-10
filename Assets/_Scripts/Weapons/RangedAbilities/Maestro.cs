using EnemyAI;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Ability
{
    //References
    private AnimationCurve animationCurve;

    //Beginnings and Targets
    private Vector3[] startPositions;
    private Vector3[] targetPos;
    private Quaternion[] startRotation;
    private Quaternion[] targetRotation;


    //Constant positions
    private Vector3 centerPos;
    private Vector3 leftCorner;
    private Vector3 rightCorner;
    private float distanceFromTarget = 8;
    private float interpolation = 10;

    //Sequence
    private float timeToFlyBack = 0.5f;
    private float timeElapsed;
    private float duration;
    private int numberOfSwings;

    // Values
    private float floatSpeed = 5;
    private float floatAmount = 0.05f;
    private bool backToHands;
    private bool left;
    private bool curve;


    public override void InitializeAbility()
    {
        base.InitializeAbility();
        range = distanceFromTarget * 2;
        startPositions = new Vector3[2];
        startRotation= new Quaternion[2];
        targetPos = new Vector3[2];
        targetRotation = new Quaternion[2];
    }

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
        animationCurve = player.currentWeapon.currentAttack.animationCurve;
        backToHands = false;
        curve = false;
        numberOfSwings = 0;
        timeElapsed = 1;
        duration = 0.4f;
        SetStartTransforms();
    }

    public override void AbilityPing()
    {
        CastBox();
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
        numberOfSwings++;
    }
    #region Sequence


    private void OneLeft()
    {
        ReleaseCurrentWeapon();
        SetStartTransforms();
        left = true;
        timeElapsed = 0;
    }
    private void TwoRight()
    {
        SetStartTransforms();
        duration = 0.7f;
        curve = true;
        left = false;
        timeElapsed = 0;
    }
    private void ThreeLeft()
    {
        SetStartTransforms();
        timeElapsed = 0;
        left = true;
    }

    private void FlyBack()
    {
        SetStartTransforms();
        backToHands = true;
        curve = false;
        timeElapsed = 0;
        duration = timeToFlyBack;
        player.InvokeMethod(Return, timeToFlyBack);
    }
    private void Return()
    {
        ReturnCurrentWeapon();
    }
    #endregion

    private void SetStartTransforms()
    {
        for (int i = 0; i < abilityTransforms.Length; i++)
        {
            startPositions[i] = abilityTransforms[i].position;
            startRotation[i] = abilityTransforms[i].rotation;
        }
    }

    #region UpdateMethods
    public override void LateUpdateAbility()
    {
        SettingTargetTransforms();
        MovingAbilityTransforms();
    }
    private void SettingTargetTransforms()
    {
        if (!backToHands)
        {
            int offset = -2;
            if(left)
            {
                offset = 2;
            }

            Vector3 directionToWeapon = (abilityTransforms[0].transform.position - player.Position() + player.Right() * offset).normalized;
            directionToWeapon.y = -10;


            Quaternion baseRotation = Quaternion.LookRotation(directionToWeapon);
            targetRotation[0] = targetRotation[1] = baseRotation /* Quaternion.Euler(baseRotation.eulerAngles.x, player.Rotation().eulerAngles.y, baseRotation.eulerAngles.z);*/;

            Vector3 targetPos = player.Position() + player.Forward() * distanceFromTarget;
            targetPos.y = player.cameraController.CameraPosition().y;

            centerPos = Vector3.Lerp(centerPos, targetPos, Time.deltaTime * interpolation);
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
            MoveAbilityTransform(abilityTransforms[i], startPositions[i], targetPos[i], startRotation[i], targetRotation[i]);
        }
    }
    private void MoveAbilityTransform(Transform abilityTrans, Vector3 startPos, Vector3 targetPos, Quaternion startRot, Quaternion targetRot)
    {
        if(timeElapsed < duration)
        {

            float t = timeElapsed / duration;

            Vector3 currentPos = Vector3.Lerp(startPos, targetPos, t);
            float y;

            if (!curve)
            {
                y = currentPos.y;
            }
            else
            {
                float remapedTime = Tools.Remap(timeElapsed, 0, duration, 0, 1);
                y = currentPos.y - animationCurve.Evaluate(remapedTime) * 1.5f;
            }

            Vector3 pos = new Vector3(currentPos.x, y, currentPos.z);

            abilityTrans.position = pos;
            abilityTrans.rotation = Quaternion.Slerp(startRot, targetRot, t);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            abilityTrans.position = targetPos + player.Right() * floatAmount * 2 * Mathf.Cos(Time.time * floatSpeed * 0.3f) + player.Up() * floatAmount * 2* Mathf.Sin(Time.time * floatSpeed);
            abilityTrans.rotation = Quaternion.Euler(targetRot.eulerAngles.x + 5 * Mathf.Cos(Time.time * floatSpeed * 0.5f), targetRot.eulerAngles.y + 10 * Mathf.Sin(Time.time * floatSpeed), targetRot.eulerAngles.z);
        }

    }
    #endregion

    private void CastBox()
    {
        List<List<Enemy>> groups = player.targetAssistance.GroupedEnemies(centerPos, new Vector3(4, 2, distanceFromTarget), player.Rotation(), new Vector3Int(3,1,3));

        for(int i = 0; i < groups.Count; i++)
        {
            for(int j = 0; j < groups[i].Count; j++)
            {
                Debug.Log("Group: " + i + " and enemy " + groups[i][j].name);
            }
        }
    }
}
