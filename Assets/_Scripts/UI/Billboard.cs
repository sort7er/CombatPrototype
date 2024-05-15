using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTrans;

    public virtual void Awake()
    {
        camTrans = Camera.main.transform;
    }

    public virtual void Update()
    {
        transform.rotation= camTrans.rotation;
    }
}
