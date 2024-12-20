using Attacks;
using EnemyAI;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Windows;

public static class Tools
{
    public static float Remap(float value, float from1 = 0, float to1 = 60, float from2 = 0, float to2 = 1)
    {
        return from2 + (value - from1) * (to2 - from2) / (to1 - from1);
    }
    public static float VolumeOfMesh(Mesh mesh)
    {
        float volume = 0;

        Vector3[] vertices = mesh.vertices;
        int[] triangles = mesh.triangles;

        for (int i = 0; i < triangles.Length; i += 3)
        {
            Vector3 p1 = vertices[triangles[i + 0]];
            Vector3 p2 = vertices[triangles[i + 1]];
            Vector3 p3 = vertices[triangles[i + 2]];
            volume += SignedVolumeOfTriangle(p1, p2, p3);
        }

        return Mathf.Abs(volume);
    }
    private static float SignedVolumeOfTriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float v321 = p3.x * p2.y * p1.z;
        float v231 = p2.x * p3.y * p1.z;
        float v312 = p3.x * p1.y * p2.z;
        float v132 = p1.x * p3.y * p2.z;
        float v213 = p2.x * p1.y * p3.z;
        float v123 = p1.x * p2.y * p3.z;

        return (1.0f / 6.0f) * (-v321 + v231 + v312 - v132 - v213 + v123);
    }

    public static Vector3 GetLocalXY(Vector3 point, Vector3 point2, Transform parent)
    {
        Vector3 globalDirection = point2 - parent.position;

        globalDirection = parent.InverseTransformDirection(globalDirection);
        globalDirection.z = 0;

        Vector3 downDir = parent.TransformDirection(globalDirection);

        return point + downDir;
    }
    public static Vector3 GetLocalY(Vector3 point, Vector3 point2, Transform parent)
    {
        Vector3 globalDirection = point2 - parent.position;

        globalDirection = parent.InverseTransformDirection(globalDirection);
        globalDirection.z = globalDirection.x = 0;

        Vector3 downDir = parent.TransformDirection(globalDirection);

        return point + downDir;
    }
    public static void SetLayerForAllChildren(GameObject parentGameObject, int layer)
    {
        parentGameObject.layer = layer;
        foreach (Transform child in parentGameObject.transform)
        {
            child.gameObject.layer = layer;

            Transform _HasChildren = child.GetComponentInChildren<Transform>();
            if (_HasChildren != null)
                SetLayerForAllChildren(child.gameObject, layer);

        }
    }
    public static float Dot(Vector3 targetDirection, Vector3 forwardDirection)
    {
        Vector3 perp = Vector3.Cross(targetDirection.normalized, forwardDirection.normalized);

        return Vector3.Dot(perp, Vector3.up);
    }
    public static bool InFront(Vector3 targetDirection, Vector3 rightDirection)
    {
        float result = Dot(targetDirection, rightDirection);

        if(result < 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public static int DirCount(string location)
    {
        DirectoryInfo d = new DirectoryInfo(location);
        return d.GetFiles().Length;
    }
    public static float LargestOfTwoFloats(float float1, float float2)
    {
        if(float1 < float2)
        {
            return float2;
        }
        else
        {
            return float1;
        }
    }
    public static float SmallestOfTwoFloats(float float1, float float2)
    {
        if (float1 > float2)
        {
            return float2;
        }
        else
        {
            return float1;
        }
    }

    #region Slash A Bit specific
    public static Attack SetUpAttack(AttackInput input)
    {
        bool right = input.activeWield == Wield.right || input.activeWield == Wield.both;
        bool left = input.activeWield == Wield.left || input.activeWield == Wield.both;

        if (right && input.attackCoordsMain.Length == 0)
        {
            input.attackCoordsMain = new AttackCoord[1];
            input.attackCoordsMain[0] = new AttackCoord(Vector3.zero, Vector3.zero);
        }

        if (left && input.attackCoordsSecondary.Length == 0)
        {
            input.attackCoordsSecondary = new AttackCoord[1];
            input.attackCoordsSecondary[0] = new AttackCoord(Vector3.zero, Vector3.zero);
        }

        return new Attack(input.animationClip, input.damage, input.postureDamage, input.activeWield, input.hitType, input.animationCurve, input.attackCoordsMain, input.attackCoordsSecondary);
    }
    public static AttackEnemy SetUpEnemyAttack(AttackEnemyInput input)
    {

        bool right = input.activeWield == Wield.right || input.activeWield == Wield.both;
        bool left = input.activeWield == Wield.left || input.activeWield == Wield.both;

        if (right && input.attackCoordsMain.Length == 0)
        {
            input.attackCoordsMain = new AttackCoord[1];
            input.attackCoordsMain[0] = new AttackCoord(Vector3.zero, Vector3.zero);
        }

        if (left && input.attackCoordsSecondary.Length == 0)
        {
            input.attackCoordsSecondary = new AttackCoord[1];
            input.attackCoordsSecondary[0] = new AttackCoord(Vector3.zero, Vector3.zero);
        }

        return new AttackEnemy(input.animationClip, input.damage, input.postureDamage, input.activeWield, input.hitType, input.animationCurve, input.attackCoordsMain, input.attackCoordsSecondary, input.exitTime, input.transitionDuration);
    }
    #endregion

}
