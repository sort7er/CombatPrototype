using System;
using UnityEngine;

[Serializable]
public class AttackCoord
{
    public Vector3 localStartPos;
    public Vector3 localEndPos;

    public Vector3 Direction(Transform relation)
    {
        return  EndPos(relation) - StartPos(relation);
    }

    public Vector3 StartPos(Transform relation)
    {
        return relation.TransformPoint(localStartPos);
    }
    public Vector3 EndPos(Transform relation)
    {
        return relation.TransformPoint(localEndPos);
    }

    public Vector3 MiddlePoint(Transform relation)
    {
        return StartPos(relation) + Direction(relation) * 0.5f;
    }
    public Vector3 NormalizedPoint(Transform relation, float point)
    {
        return StartPos(relation) + Direction(relation) * point;
    }
}
