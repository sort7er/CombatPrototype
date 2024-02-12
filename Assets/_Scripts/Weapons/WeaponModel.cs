using UnityEngine;

public class WeaponModel : MonoBehaviour
{
    protected Vector3 lastPos, newPos;
    protected Vector3 direction;

    [SerializeField] protected Transform endPoint;
    [SerializeField] protected Transform startPoint;

    protected virtual void Update()
    {
        newPos = endPoint.position;
        direction = newPos - lastPos;
        lastPos = endPoint.position;
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
    public virtual void SwingDone()
    {

    }
    public void SetParent(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public void Hit()
    {
        Vector3 planeNormal = Vector3.Cross(endPoint.position - startPoint.position, direction);
        planeNormal.Normalize();

        EffectManager.instance.Hit(endPoint.position, direction, planeNormal);
    }
}
