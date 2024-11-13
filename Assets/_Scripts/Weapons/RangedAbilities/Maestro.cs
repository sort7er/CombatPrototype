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
    private float timeToFlyBack = 0.7f;
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
    private AnimationCurve middleCurve;
    private AnimationCurve startCurve;
    private Vector3 enemyPos;
    private bool isMiddleCurve;
    private float enemyGroundOffset = 1.5f;
    private float weaponBackOffset = 1f;
    private float rightAmount;
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
        middleCurve = new AnimationCurve();
        startCurve = new AnimationCurve();
        backToHands = false;
        isMiddleCurve = false;
        numberOfSwings = 0;
        timeElapsed = 1;
        duration = 0.7f;
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
        isMiddleCurve = true;
        duration = 0.7f;
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
        isMiddleCurve = false;
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
            float remapedTime = Tools.Remap(timeElapsed, 0, duration, 0, 1);

            float x;
            float y;
            float z;

            if (!isMiddleCurve)
            {
                x = localCurrentPos.x - startCurve.Evaluate(remapedTime) * rightAmount; 
                y = localCurrentPos.y - startCurve.Evaluate(remapedTime) * downwardsAmount;
                z = localCurrentPos.z;

            }
            else
            {
                x = localCurrentPos.x;
                y = localCurrentPos.y - middleCurve.Evaluate(remapedTime) * downwardsAmount;
                z = localCurrentPos.z - middleCurve.Evaluate(remapedTime) * backwardsAmount;
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
        List<List<Enemy>> groups = player.targetAssistance.GroupedEnemies(centerPos, new Vector3(5, 6, distanceFromTarget * 2), player.Rotation(), new Vector3Int(3,2,4));

        if(groups.Count > 0 )
        {
            enemyPos = AveragePosOfGroup(groups[0]);

            //for (int j = 0; j < groups[0].Count; j++)
            //{
            //    Debug.Log("Group " + 0 + " and " + groups[0][j].name);

            //}

            for (int i = 0; i < groups.Count; i++)
            {
                //for (int j = 0; j < groups[i].Count; j++)
                //{
                //    Debug.Log("Group " + i + " and " + groups[i][j].name);

                //}
                //Debug.Log("Group " + i + " Distance " + Vector3.Distance(AveragePosOfGroup(groups[i]), playerTrans.position));
            }

            enemyPos.y += enemyGroundOffset;

            downwardsAmount = leftCorner.y - enemyPos.y;

            if (isMiddleCurve)
            {
                CalculateMiddleCurve();
            }
            else
            {
                CalculateStartCurve();
            }

        }
        else
        {
            middleCurve.keys = baseCurve.keys;
            backwardsAmount = 0;
            downwardsAmount = 1.5f;
        }
    }

    private void CalculateMiddleCurve()
    {
        Vector3 localEnemyPos = playerTrans.InverseTransformPoint(enemyPos);
        Vector3 localRightCorner = playerTrans.InverseTransformPoint(rightCorner);
        Vector3 localLeftCorner = playerTrans.InverseTransformPoint(leftCorner);

        float xDifference;    

        if (!left)
        {
            xDifference = Mathf.Abs(localRightCorner.x - localEnemyPos.x);
        }
        else
        {
            xDifference = Mathf.Abs(localLeftCorner.x - localEnemyPos.x);
        }

        //This is the same no matter left or right
        float normalizedX = Tools.Remap(xDifference, 0, offsets * 2, 1, 0);

        backwardsAmount = localLeftCorner.z - localEnemyPos.z + weaponBackOffset;

        Keyframe[] modifiedKeys = new Keyframe[baseCurve.length];

        modifiedKeys[0] = baseCurve.keys[0];
        modifiedKeys[1] = new Keyframe(normalizedX, 1);
        modifiedKeys[2] = baseCurve.keys[2];

        middleCurve.keys = modifiedKeys;
    }

    private void CalculateStartCurve()
    {
        Vector3 localEnemyPos = playerTrans.InverseTransformPoint(enemyPos);
        Vector3 localLeftCorner = playerTrans.InverseTransformPoint(leftCorner);


        float normalizedZ;
        float crockedLineAjustment;
        backwardsAmount = localLeftCorner.z - localEnemyPos.z + weaponBackOffset;
        normalizedZ = Tools.Remap(backwardsAmount, 0, distanceFromTarget, 0, 1);

        crockedLineAjustment = Tools.Remap(normalizedZ, 0, 1, 0, 5);

        rightAmount = localLeftCorner.x - localEnemyPos.x + crockedLineAjustment;

        //if (Tools.Dot(localEnemyPos, Vector3.forward) > 0)
        //{
        //    //Left

        //    normalizedZ = Tools.Remap(backwardsAmount, 0, distanceFromTarget, 0, 1);
        //    leftAmount =  localLeftCorner.x - localEnemyPos.x * (1 + normalizedZ);
        //}
        //else
        //{
        //    //Right

        //    normalizedZ = Tools.Remap(backwardsAmount, 0, distanceFromTarget, 0, 1);
        //    leftAmount = - localEnemyPos.x * (1 + normalizedZ);
        //}


        //This works if target is on the left side




        Keyframe[] modifiedKeys = new Keyframe[baseCurve.length];

        modifiedKeys[0] = baseCurve.keys[0];
        modifiedKeys[1] = new Keyframe(normalizedZ, 1);
        modifiedKeys[2] = baseCurve.keys[2];

        startCurve.keys = modifiedKeys;



    }
    private Vector3 AveragePosOfGroup(List<Enemy> enemies)
    {

        Vector3 targetPos = Vector3.zero;
        for(int i = 0; i < enemies.Count; i++)
        {
            targetPos += enemies[i].Position();
        }

        return targetPos / enemies.Count;
    } 
}
