using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Stats.CharacterStats
{
    public class Health : MonoBehaviour
    {
        [SerializeField]
        private float maxHp;
        public float currentHp;

        [SerializeField]
        private float maxShield;
        public float currentShield;

        private Animator animator;
        private int beingHitHash;
        private int stopBeingHitHash;
        [SerializeField]
        private AnimatorOverrideController animatorOverrideController;
        private AnimatorOverrideController defaultAnimatorOverrideController;
        private bool isDead;

        [SerializeField]
        private float timeRestoreShield;
        private float timeRestoreCurrent;

        private void Start()
        {
            animator = GetComponent<Animator>();
            beingHitHash = Animator.StringToHash("beingHit");
            stopBeingHitHash = Animator.StringToHash("stopBeingHit");
            defaultAnimatorOverrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            currentHp = maxHp;
            currentShield = maxShield;
        }

        public void TakeDame(float dame)
        {
            //Caculate dame taken
            if (isShieldAvailable())
            {
                currentShield -= 2f * dame;
            } else
            {
                currentHp -= dame;
                ResetShield();
            }

            //Check state
            if (currentHp <= 0)
            {
                Dead();
            }

            currentHp = Mathf.Clamp(currentHp, 0, maxHp);

            if (currentHp <= maxHp * 0.8f)
            {
                animator.runtimeAnimatorController = animatorOverrideController;
            }
            else
            {
            }
        }

        private void ResetShield()
        {
            if (isShieldAvailable()) return;

            if (timeRestoreCurrent >= timeRestoreShield)
            {
                timeRestoreCurrent = 0;
                currentShield = maxShield;
                animator.ResetTrigger(beingHitHash);
            }

            timeRestoreCurrent += Time.deltaTime;
        }

        //if shield is broken, is called when being hit (hit animation)
        public void ShieldBrokeAction()
        {
            if (isShieldAvailable()) return;

            animator.SetTrigger(beingHitHash);
        }


        private bool isShieldAvailable()
        {
            return currentShield > 0;
        }

        private void Dead()
        {
            if (isDead) return;

            isDead = true;
            animator.SetTrigger("death");
            GetComponent<ActionSchedular>().CancelAction();
        }

        public bool IsDead()
        {
            return isDead;
        }

    }
}
