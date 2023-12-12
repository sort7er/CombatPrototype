using UnityEngine;

public class Enemy : Humanoid
{

    public void AddForce(Vector3 force)
    {
        rb.AddForce(force, ForceMode.Impulse);
    }

}
