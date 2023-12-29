using UnityEngine;

public class ParentManager : MonoBehaviour
{
    public static ParentManager instance;

    public Transform effects;
    public Transform meshes;




    private void Awake()
    {
        instance = this;
    }
}
