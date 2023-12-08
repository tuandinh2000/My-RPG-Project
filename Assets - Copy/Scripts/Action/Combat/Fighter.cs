using RPG.Action.Movement;
using RPG.Controller;
using RPG.Stats.CharacterStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Action.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        //Component field
        private ActionSchedular actionSchedular;
        private Animator animator;
        private Mover mover;

        [SerializeField]
        private float timeBetweenAttack;
        private float lastTimeAttack;

        [SerializeField]
        private float weaponAttackRange = 5f;
        private Health target;
        

        //Animation Hash
        private int attackHash;
        private int stopAttackHash;
        private int attackPatternHash;
        private int attackPatternCountHash;
        private int isStanceHash;
        
        private int attackPatternCount;

        // Start is called before the first frame update
        void Start()
        {
            animator = GetComponent<Animator>();
            actionSchedular = GetComponent<ActionSchedular>();
            mover = GetComponent<Mover>();

            attackHash = Animator.StringToHash("attack");
            stopAttackHash = Animator.StringToHash("stopAttack");
            attackPatternHash = Animator.StringToHash("attackPattern");
            attackPatternCountHash = Animator.StringToHash("attackPatternCount");
            isStanceHash = Animator.StringToHash("isStance");
        }

        // Update is called once per frame
        void Update()
        {
            if (target == null) return;
            if (target.IsDead()) return;

            if (!IsTargetInAttackRange())
            {
                mover.MoveToDestination(target.transform.position, 1);
            } else
            {
                mover.CancelAction();

                if (GetComponent<PlayerController>())
                {
                    AttackPatternBehaviour();
                } else
                {
                    AttackBehaviour();
                }
            }
        }

        public bool IsAttackable(GameObject combatTarget)
        {
            return !combatTarget.GetComponent<Health>().IsDead();
        }

        public void StartAttackAction(GameObject combatTarget)
        {
            actionSchedular.StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        private void AttackBehaviour()
        {
            transform.LookAt(target.transform.position);

            if (lastTimeAttack < Time.time)
            {
                animator.SetTrigger(attackHash);
                lastTimeAttack = Time.time + timeBetweenAttack;
            }
        }

        private void AttackPatternBehaviour()
        {
            transform.LookAt(target.transform.position);

            if (lastTimeAttack < Time.time)
            {
                animator.SetTrigger(attackPatternHash);
                ResetAttackCount();
                lastTimeAttack = Time.time + timeBetweenAttack;
            }
        }

        private void ResetAttackCount()
        {
            animator.SetInteger(attackPatternCountHash, attackPatternCount);
            attackPatternCount++;
            if (attackPatternCount > 1) attackPatternCount = 0;
        }

        private bool IsTargetInAttackRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < weaponAttackRange;
        }

        public void CancelAction()
        {
            mover.CancelAction();
            target = null;
            attackPatternCount = 0;
            animator.SetInteger(attackPatternCountHash, attackPatternCount);
            animator.SetTrigger(stopAttackHash);
            animator.ResetTrigger(attackHash);
            animator.ResetTrigger(attackPatternHash);
        }

        //Animation Event
        void Hit()
        {
            if (target == null) return;

            target.TakeDame(5);
            target.ShieldBrokeAction();
        }
    }
}
