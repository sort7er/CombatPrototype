using UnityEngine;

public class Target
{
    public Transform targetTransform;
    public float dotProduct;
    public float distance;

    public bool insideTarget;

    public Target(Transform targetTransform, float dotProduct, float distance, ref bool insideTarget)
    {
        this.targetTransform = targetTransform;
        this.dotProduct = dotProduct;
        this.distance = distance;
        this.insideTarget = insideTarget;
    }
}
