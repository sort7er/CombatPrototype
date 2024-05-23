using UnityEngine;
using Attacks;

public class WeaponModel : MonoBehaviour
{

    [Header("References")]
    [SerializeField] protected Weapon weapon;
    [SerializeField] protected Transform effectTrans;

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

    public virtual void Attack()
    {

    }

    public void SetAttackCoord(AttackCoord attackCoord)
    {
        this.attackCoord = attackCoord;
    }

    public virtual void Effect()
    {
        if(weapon.archetype.showStartPos)
        {
            DisplayAttackCoords("Start");
        }

        if(weapon.archetype.showEffect)
        {

            if (weapon.currentAttack.hitType == HitType.slice)
            {
                Vector3 ajustedPosition = transform.position;

                ajustedPosition.y = weapon.transform.position.y;

                Vector3 planeNormal = Vector3.Cross(weapon.transform.forward, attackCoord.Direction(weapon.transform));
                planeNormal.Normalize();

                EffectManager.instance.Slash(weapon.archetype.slashEffect, attackCoord.MiddlePoint(weapon.transform), weapon.transform.forward, planeNormal, weapon.transform, weapon.archetype.effectSize);
            }
            else
            {
                EffectManager.instance.Thrust(attackCoord.EndPos(weapon.transform), attackCoord.Direction(weapon.transform), weapon.transform.up, weapon.transform, weapon.archetype.effectSize);
            }
        }

    }

    public virtual void AttackDone()
    {
        if(weapon.archetype.showEndPos)
        {
            DisplayAttackCoords("End");
        }
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


        EffectManager.instance.Hit(hitPoint, attackCoord.Direction(weapon.transform), planeNormal);
    }

    private void DisplayAttackCoords(string prefix)
    {
        Transform par = effectTrans.parent;
        effectTrans.parent = weapon.transform;
        Vector3 localPos = effectTrans.localPosition;

        string copyToClipboard = "Vector3(" + CommaToDot(localPos.x) + ", " + CommaToDot(localPos.y) + ", " + CommaToDot(localPos.z) + ")";
        GUIUtility.systemCopyBuffer = copyToClipboard;

        //Debug.Log(prefix);
        //Debug.Log(copyToClipboard);

        Debug.Log(transform.name + ": " + weapon.currentAttack.animationClip.name + ", " + prefix + ": " + localPos);
        effectTrans.parent = par;
    }

    private string CommaToDot(float num)
    {
        return num.ToString("F2").Replace(",", ".");
    }

}
