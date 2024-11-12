using EnemyAI;
using System.Collections.Generic;
using UnityEngine;

public class Maestro : Ability
{
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
    private float offsets = 5;

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


    //Curving
    private AnimationCurve baseCurve;
    private AnimationCurve modifiedCurve;
    private Vector3 enemyPos;
    private bool isCurving;
    private float enemyGroundOffset = 1.5f;
    private float weaponBackOffset = 1f;
    private float downwardsAmount;
    private float backwardsAmount;


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
        baseCurve = player.currentWeapon.currentAttack.animationCurve;
        modifiedCurve = new AnimationCurve();
        backToHands = false;
        isCurving = false;
        numberOfSwings = 0;
        timeElapsed = 1;
        duration = 0.4f;
        SetStartTransforms();
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
        CastBox();
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
        isCurving = true;
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
        isCurving = false;
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
            targetRotation[0] = targetRotation[1] = baseRotation;

            Vector3 targetPos = player.Position() + player.Forward() * distanceFromTarget;
            targetPos.y = player.cameraController.CameraPosition().y;

            centerPos = Vector3.Lerp(centerPos, targetPos, Time.deltaTime * interpolation);
            leftCorner = centerPos - player.Right() * offsets + player.Up();
            rightCorner = centerPos + player.Right() * offsets + player.Up();

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

            Vector3 localStartPos = playerTrans.InverseTransformPoint(startPos);
            Vector3 localTargetPos = playerTrans.InverseTransformPoint(targetPos);

            Vector3 localCurrentPos = Vector3.Lerp(localStartPos, localTargetPos, t);
            float x;
            float y;
            float z;

            if (!isCurving)
            {
                x = localCurrentPos.x; 
                y = localCurrentPos.y;
                z = localCurrentPos.z;

            }
            else
            {
                float remapedTime = Tools.Remap(timeElapsed, 0, duration, 0, 1);

                x = localCurrentPos.x;
                y = localCurrentPos.y - modifiedCurve.Evaluate(remapedTime) * downwardsAmount;
                z = localCurrentPos.z - modifiedCurve.Evaluate(remapedTime) * backwardsAmount;
            }

            Vector3 pos = playerTrans.TransformPoint(new Vector3(x, y, z));

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
        List<List<Enemy>> groups = player.targetAssistance.GroupedEnemies(centerPos, new Vector3(4, 4, distanceFromTarget), player.Rotation(), new Vector3Int(3,2,3));

        if(groups.Count > 0 )
        {
            enemyPos = GroupToTarget(groups[0]);
            enemyPos.y += enemyGroundOffset;

            downwardsAmount = leftCorner.y - enemyPos.y;

            Vector3 localLeftCorner = playerTrans.InverseTransformPoint(leftCorner);
            Vector3 localEnemyPos = playerTrans.InverseTransformPoint(enemyPos);

            backwardsAmount = localLeftCorner.z - localEnemyPos.z + weaponBackOffset;

            float xDifference = Mathf.Abs(localLeftCorner.x - localEnemyPos.x);

            float normalizedX = Tools.Remap(xDifference, 0, offsets * 2, 0, 1);

            Keyframe[] modifiedKeys = new Keyframe[baseCurve.length];

            modifiedKeys[0] = baseCurve.keys[0];
            modifiedKeys[1] = new Keyframe(normalizedX, 1);
            modifiedKeys[2] = baseCurve.keys[2];

            modifiedCurve.keys = modifiedKeys;

        }
        else
        {
            modifiedCurve.keys = baseCurve.keys;
            backwardsAmount = 0;
            downwardsAmount = 1.5f;
        }
    }

    private Vector3 GroupToTarget(List<Enemy> enemies)
    {

        Vector3 targetPos = Vector3.zero;
        for(int i = 0; i < enemies.Count; i++)
        {
            targetPos += enemies[i].Position();
        }

        return targetPos / enemies.Count;
    } 
}
