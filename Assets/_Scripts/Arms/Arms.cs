using UnityEngine;

public class Arms : MonoBehaviour
{
    public Animator armAnimator;
    public Weapon currentWeapon { get; private set; }






    //Called from state machine
    public void SetAnimation(Anim newAnim, float transition = 0.25f)
    {
        armAnimator.CrossFade(newAnim.state, transition);
    }

    //Called from other scripts
    public void SetNewWeapon(Weapon weapon)
    {
        currentWeapon = weapon;
    }

}
