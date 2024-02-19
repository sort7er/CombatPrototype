using UnityEngine;
using Attacks;

public class WeaponModel : MonoBehaviour
{

    [Header("References")]
    [SerializeField] protected Weapon weapon;

    //public Transform arrow;

    protected AttackCoord attackCoord;

    public Vector3 Position()
    {
        return transform.position;
    }

    public Vector3 UpDir()
    {
        return transform.up;
    }

    public virtual void Attack(AttackCoord attackCoord)
    {
        this.attackCoord = attackCoord;
    }
    public void Effect()
    {
        //Transform par = transform.parent;
        //transform.parent = weapon.transform;
        //Debug.Log(transform.name + ": " + weapon.currentAttack.animationClip.name + ", Start: " + transform.localPosition);
        //transform.parent = par;

        //if (weapon.currentAttack.hitType == HitType.slice)
        //{
        //    Vector3 planeNormal = Vector3.Cross(transform.position - weapon.transform.position, attackCoord.Direction(weapon.transform));
        //    planeNormal.Normalize();

        //    EffectManager.instance.Slash(attackCoord.MiddlePoint(weapon.transform), weapon.transform.forward, planeNormal, weapon.transform);
        //}
        //else
        //{
        //    EffectManager.instance.Thrust(attackCoord.EndPos(weapon.transform), attackCoord.Direction(weapon.transform), weapon.transform.up, weapon.transform);
        //}
    }

    public virtual void AttackDone()
    {
        //Transform par = transform.parent;
        //transform.parent = weapon.transform;
        //Debug.Log(transform.name + ": " + weapon.currentAttack.animationClip.name + ", End: " + transform.localPosition);
        //transform.parent = par;
    }
    public void SetParent(Transform parent)
    {
        transform.parent = parent;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }
    public void Hit(Vector3 hitPoint)
    {
        Vector3 planeNormal = Vector3.Cross(transform.position - weapon.transform.position, attackCoord.Direction(weapon.transform));
        planeNormal.Normalize();

        //arrow.position = transform.position;
        //arrow.rotation = Quaternion.LookRotation(attackCoord.Direction(weapon.transform));

        EffectManager.instance.Hit(hitPoint, attackCoord.Direction(weapon.transform), planeNormal);
    }
}
