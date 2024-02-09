using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    protected Vector3 lastPos, newPos;
    protected Vector3 direction;

    protected virtual void Update()
    {
        newPos = transform.position;
        direction = newPos - lastPos;
        lastPos = transform.position;
    }

    public Vector3 Position()
    {
        return transform.position;
    }
    public Vector3 Direction()
    {
        return direction;
    }
    public Vector3 UpDir()
    {
        return transform.up;
    }

    public void SetParent(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

}
