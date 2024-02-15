using UnityEngine;

public class WeaponModel : MonoBehaviour
{

    [Header("References")]
    [SerializeField] protected Weapon weapon;

    //[SerializeField] protected Transform endPoint;
    //[SerializeField] protected Transform startPoint;

    protected Vector3 startPos;

    //public Transform arrow;

    public Vector3 Position()
    {
        return transform.position;
    }
    public Vector3 Direction()
    {
        return transform.position - startPos;
    }
    public Vector3 UpDir()
    {
        return transform.up;
    }
    public void SetAttackStartPoint()
    {
        startPos = transform.position;
    }
    public virtual void AttackDone()
    {

    }
    public void SetParent(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public void Hit(Vector3 hitPoint)
    {
        Vector3 planeNormal = Vector3.Cross(transform.position - weapon.transform.position, Direction());
        planeNormal.Normalize();

        //arrow.position = transform.position;
        //arrow.rotation = Quaternion.LookRotation(Direction());

        EffectManager.instance.Hit(hitPoint, Direction(), planeNormal);
    }
}
