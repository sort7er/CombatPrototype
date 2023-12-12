using System;

[Serializable]
public class TargetAssistanceParams
{
    public float range = 10f;
    public float idealDotProduct = 0.85f;
    public float acceptedDotProduct = 0.75f;
}
