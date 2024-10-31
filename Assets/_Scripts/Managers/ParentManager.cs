using UnityEngine;

public class ParentManager : MonoBehaviour
{
    public static ParentManager instance;

    public Transform effects;
    public Transform meshes;
    public Transform abilities;




    private void Awake()
    {
        instance = this;
    }
}
