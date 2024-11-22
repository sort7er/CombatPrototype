using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MaestroMovement
{

    public float timeElapsed { get; private set; } 

    private Maestro maestro;
    private Player player;
    private Transform playerTrans;
    private Transform[] abilityTransforms;

    //Curves
    private AnimationCurve baseCurve;
    private AnimationCurve middleCurve;
    private AnimationCurve startCurve;
    private Vector3 enemyPos;
    private float enemyGroundOffset = 1.5f;
    private float weaponBackOffset = 1f;
    private float weaponRightOffset = 0.5f;
    private float rightAmount;
    private float downwardsAmount;
    private float backwardsAmount;

    //Beginnings and Targets
    private Vector3[] startPositions;
    private Vector3[] targetPos;
    private Quaternion[] startRotation;
    private Quaternion[] targetRotation;

    //Constant positions
    private Vector3 centerPos;
    private Vector3 leftCorner;
    private Vector3 rightCorner;
    private float distanceFromTarget = 10;
    private float interpolation = 10;
    private float offsets = 5;
    private float centerOffset = 6;

    //Floating
    private float floatSpeed = 5;
    private float floatAmount = 0.05f;

    public MaestroMovement(Maestro maestro)
    {
        this.maestro = maestro;
        player = maestro.player;
        playerTrans = maestro.playerTrans;
        abilityTransforms = maestro.abilityTransforms;

        startPositions = new Vector3[2];
        startRotation = new Quaternion[2];
        targetPos = new Vector3[2];
        targetRotation = new Quaternion[2];

        baseCurve = player.currentWeapon.currentAttack.animationCurve;
        middleCurve = new AnimationCurve();
        startCurve = new AnimationCurve();
    }

    public void SetStartValues()
    {
        timeElapsed = 1;
    }

    public void ResetTimeElapsed()
    {
        timeElapsed = 0;
    }
    public void SetStartTransforms()
    {
        for (int i = 0; i < abilityTransforms.Length; i++)
        {
            startPositions[i] = abilityTransforms[i].position;
            startRotation[i] = abilityTransforms[i].rotation;
        }
    }
    public void SettingTargetTransforms()
    {
        if (!maestro.backToHands)
        {
            int offset = -2;
            if (maestro.isLeft)
            {
                offset = 2;
            }

            Vector3 directionToWeapon = (abilityTransforms[0].transform.position - player.Position() + player.Right() * offset).normalized;
            directionToWeapon.y = -3;


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
        if (maestro.isLeft)
        {

            targetPos[0] = targetPos[1] = leftCorner;
        }
        else
        {
            targetPos[0] = targetPos[1] = rightCorner;
        }
    }
    public void MovingAbilityTransforms()
    {
        for (int i = 0; i < abilityTransforms.Length; i++)
        {
            MoveAbilityTransform(abilityTransforms[i], startPositions[i], targetPos[i], startRotation[i], targetRotation[i]);
        }
    }
    private void MoveAbilityTransform(Transform abilityTrans, Vector3 startPos, Vector3 targetPos, Quaternion startRot, Quaternion targetRot)
    {
        if (timeElapsed < maestro.duration)
        {

            float t = timeElapsed / maestro.duration;

            Vector3 localStartPos = playerTrans.InverseTransformPoint(startPos);
            Vector3 localTargetPos = playerTrans.InverseTransformPoint(targetPos);

            Vector3 localCurrentPos = Vector3.Lerp(localStartPos, localTargetPos, t);
            float remapedTime = Tools.Remap(timeElapsed, 0, maestro.duration, 0, 1);

            float x;
            float y;
            float z;

            if (!maestro.isMiddleCurve)
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
            abilityTrans.position = targetPos + player.Right() * floatAmount * 2 * Mathf.Cos(Time.time * floatSpeed * 0.3f) + player.Up() * floatAmount * 2 * Mathf.Sin(Time.time * floatSpeed);
            abilityTrans.rotation = Quaternion.Euler(targetRot.eulerAngles.x + 5 * Mathf.Cos(Time.time * floatSpeed * 0.5f), targetRot.eulerAngles.y + 10 * Mathf.Sin(Time.time * floatSpeed), targetRot.eulerAngles.z);
        }

    }



    public void CastBox()
    {
        List<TargetGroup> groups = player.targetAssistance.GroupedEnemies(BoxCenter(), BoxSize(), player.Rotation(), new Vector3Int(3, 2, 4));

        if (groups.Count > 0)
        {
            enemyPos = groups[0].AveragePosOfGroup();

            enemyPos.y += enemyGroundOffset;

            downwardsAmount = leftCorner.y - enemyPos.y;

            if (maestro.isMiddleCurve)
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
            rightAmount = 0;

            if (!maestro.isMiddleCurve)
            {
                downwardsAmount = 0f;
            }
            else
            {
                downwardsAmount = 1.5f;
            }
        }
    }

    public Vector3 BoxCenter()
    {
        Vector3 ajustedCenter = playerTrans.InverseTransformPoint(centerPos);
        ajustedCenter.z -= centerOffset * 0.5f;

        return playerTrans.TransformPoint(ajustedCenter);
    } 
    public Vector3 BoxSize()
    {
        float length = (distanceFromTarget * 2 - centerOffset) * 0.5f;
        return new Vector3(5, 6, length);
    }

    private void CalculateMiddleCurve()
    {
        Vector3 localEnemyPos = playerTrans.InverseTransformPoint(enemyPos);
        Vector3 localRightCorner = playerTrans.InverseTransformPoint(rightCorner);
        Vector3 localLeftCorner = playerTrans.InverseTransformPoint(leftCorner);

        float xDifference;

        if (!maestro.isLeft)
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

        if (!maestro.backToHands)
        {
            normalizedZ = 1 - normalizedZ;
        }


        crockedLineAjustment = Tools.Remap(normalizedZ, 0, 1, 0, 5);

        rightAmount = localLeftCorner.x - localEnemyPos.x + crockedLineAjustment - weaponRightOffset;

        //Debug.Log("Right amout " + rightAmount + ". Difference " + (localLeftCorner.x - localEnemyPos.x) + ". Crocked " + crockedLineAjustment);

        Keyframe[] modifiedKeys = new Keyframe[baseCurve.length];

        modifiedKeys[0] = baseCurve.keys[0];
        modifiedKeys[1] = new Keyframe(normalizedZ, 1);
        modifiedKeys[2] = baseCurve.keys[2];



        startCurve.keys = modifiedKeys;

    }
}
