using System;
using System.Collections;
using UnityEngine;

namespace Actions
{
    public class PlayerActions : MonoBehaviour
    {
        [Header("Values")]
        public float blockWaitTime = 0;

        [Header("References")]
        public Animator armAnimator;
        public Player player;
        public Unique unique;

        public Transform[] weaponPos;
        public Weapon currentWeapon { get; private set; }
        public ActionState currentState { get; private set; }
        public Anim currentAnimation { get; private set; }
        public bool isMoving { get; private set; }
        public bool isFalling { get; private set; }

        public IdleState idleState = new IdleState();
        public JumpState jumpState = new JumpState();
        public FallState fallState = new FallState();
        public AttackState attackState = new AttackState();
        public UniqueState uniqueState = new UniqueState();
        public BlockState blockState = new BlockState();
        public ParryState parryState = new ParryState();
        public PerfectParryState perfectParryState = new PerfectParryState();
        public ParryAttackState parryAttackState = new ParryAttackState();

        private InputReader inputReader;



        public int currentParry { get; private set; }
        public int currentPerfectParry { get; private set; }
        public float uniqueCoolDown { get; private set; } = 8f;
        [HideInInspector] public bool canUseUnique = true;


        private void Awake()
        {
            SetUpInput();
        }
        private void Start()
        {
            SwitchState(idleState);
        }

        private void OnDestroy()
        {
            EndInput();
        }

        #region Signal states
        private void Update()
        {
            currentState.Update();
        }
        public void Moving()
        {
            isMoving = true;
            currentState.Moving();
        }
        public void StoppedMoving()
        {
            isMoving = false;
            currentState.StoppedMoving();
        }
        public void Jumping()
        {
            isFalling = true;
            currentState.Jump();
        }
        public void Fall()
        {
            isFalling = true;
            currentState.Fall();
        }
        public void Landing()
        {
            isFalling = false;
            currentState.Landing();
        }
        public void Attack()
        {
            currentState.Attack();
        }
        public void Unique()
        {
            if(canUseUnique)
            {
                currentState.Unique();
            }
        }
        public void Block()
        {
            currentState.Block();
        }
        public void BlockRelease()
        {
            currentState.BlockRelease();
        }
        public void Parry()
        {
            currentState.Parry();
        }
        public void PerfectParry()
        {
            currentState.PerfectParry();
        }
        public void ActionStart()
        {
            currentWeapon.Effect();
        }
        public void OverlapCollider()
        {
            currentState.OverlapCollider();
        }
        public void ActionDone()
        {
            currentState.ActionDone();
            currentWeapon.AttackDone();
        }

        #endregion


        //Called from state machine

        public void StartUniqueCooldown()
        {
            unique.Loading();
            Invoke(nameof(UniqueCooldownDone), uniqueCoolDown);
        }
        private void UniqueCooldownDone()
        {
            unique.Active();
            canUseUnique = true;
        }
        public void SwitchState(ActionState state)
        {
            currentState = state;
            currentState.Enter(this);
        }

        public void SetMovement(Vector2 movement)
        {
            armAnimator.SetFloat("MovementX", movement.x);
            armAnimator.SetFloat("MovementZ", movement.y);
        }
        public void SetAnimation(Anim newAnim, float transition = 0.25f)
        {
            if (newAnim is Attack attack)
            {
                currentWeapon.Attack(attack);
            }
            else
            {
                currentWeapon.NoAttack();
            }


            currentAnimation = newAnim;
            armAnimator.CrossFadeInFixedTime(currentAnimation.state, transition);
        }
        public int GetCurrentParry()
        {
            if (currentParry == 0)
            {
                currentParry = 1;
            }
            else
            {
                currentParry = 0;
            }
            return currentParry;
        }
        public void SetCurrentPerfectParry()
        {
            if (currentPerfectParry == 0)
            {
                currentPerfectParry = 1;
            }
            else
            {
                currentPerfectParry = 0;
            }
        }
        #region Invoking
        public void InvokeMethod(Action function, float waitTime)
        {
            StartCoroutine(DoFunction(function, waitTime));
        }
        private IEnumerator DoFunction(Action function, float waitTime)
        {
            yield return new WaitForSeconds(waitTime);
            function.Invoke();
        }
        public void StopMethod()
        {
            StopAllCoroutines();
        }
        #endregion

        //Called from other classes
        public void SetNewWeapon(Weapon weapon)
        {
            CancelInvoke(nameof(Delay));
            if (currentWeapon != null)
            {
                currentWeapon.Hidden();
            }

            currentWeapon = weapon;
            currentWeapon.SetOwner(player, player.cameraController.camTrans, weaponPos);
            player.hitBox.SetCurrentWeapon();
            currentWeapon.Vissible();
            Invoke(nameof(Delay), 0.02f);
        }

        private void Delay()
        {
            SwitchState(idleState);
        }

        public bool CanSwitchWeapon()
        {
            if(currentState == attackState || currentState == parryState || currentState == blockState || currentState == uniqueState)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #region Redirects
        private void SetUpInput()
        {
            inputReader = player.inputReader;

            inputReader.OnAttack += Attack;
            inputReader.OnUnique += Unique;
            inputReader.OnBlock += Block;
            inputReader.OnMoveStarted += Moving;
            inputReader.OnMoveStopped += StoppedMoving;
            inputReader.OnBlockRelease += BlockRelease;

            player.OnJump += Jumping;
            player.OnFalling += Fall;
            player.OnLanding += Landing;
        }
        private void EndInput()
        {
            inputReader.OnAttack -= Attack;
            inputReader.OnUnique -= Unique;
            inputReader.OnBlock -= Block;
            inputReader.OnMoveStarted -= Moving;
            inputReader.OnMoveStopped -= StoppedMoving;
            inputReader.OnBlockRelease -= BlockRelease;

            player.OnJump -= Jumping;
            player.OnFalling -= Fall;
            player.OnLanding -= Landing;
        }

        #endregion
    }
}

