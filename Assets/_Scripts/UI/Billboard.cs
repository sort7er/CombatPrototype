using UnityEngine;

public class Billboard : MonoBehaviour
{
    private Transform camTrans;

    private void Awake()
    {
        camTrans = Camera.main.transform;
    }

    void Update()
    {
        transform.rotation= camTrans.rotation;
    }
}
