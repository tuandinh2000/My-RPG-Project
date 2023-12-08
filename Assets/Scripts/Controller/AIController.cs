using RPG.Action.Combat;
using RPG.Action.Movement;
using RPG.Stats.CharacterStats;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Controller
{
    public class AIController : MonoBehaviour
    {
        private int currentPatrolPoint;

        [SerializeField]
        private float chaseDistance;

        [SerializeField]
        private float suspiciousTime;
        private float lastTimeSeenTarget = Mathf.Infinity;
        private bool isLooking;

        private GameObject target;
        private Fighter fighter;
        private Mover mover;
        private Animator animator;

        [SerializeField]
        private PatrolPath patrolPath;
        private int lookingHash;
        private int quitLookingHash;


        // Start is called before the first frame update
        void Start()
        {
            target = FindObjectOfType<PlayerController>().gameObject;
            fighter = GetComponent<Fighter>();
            mover = GetComponent<Mover>();
            animator = GetComponent<Animator>();

            lookingHash = Animator.StringToHash("looking");
            quitLookingHash = Animator.StringToHash("quitLooking");
        }

        // Update is called once per frame
        void Update()
        {
            if (GetComponent<Health>().IsDead()) return;
            if (target == null) return;

            if (IsTargetInChasingRange() && fighter.IsAttackable(target))
            {
                AttackBehaviour();
            } else if (lastTimeSeenTarget < suspiciousTime)
            {
                SuspiciousBehaviour();
            }
            else
            {
                isLooking = false;
                PatrolBehaviour();
            }

            lastTimeSeenTarget += Time.deltaTime;
        }

        private bool IsTargetInChasingRange()
        {
            return Vector3.Distance(transform.position, target.transform.position) < chaseDistance;
        }

        private void AttackBehaviour()
        {
            if (isLooking)
            {
                isLooking = false;
                animator.SetTrigger(quitLookingHash);
            }

            animator.ResetTrigger(lookingHash);
            lastTimeSeenTarget = 0;
            fighter.StartAttackAction(target);
        }

        private void SuspiciousBehaviour()
        {
            if (isLooking) return;
            isLooking = true;

            GetComponent<ActionSchedular>().CancelAction();
            animator.SetTrigger(lookingHash);
            animator.ResetTrigger(quitLookingHash);
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = transform.position;

            if (patrolPath != null)
            {
                if (IsMoveToNextPatrolPoint())
                {
                    UpdateCurrentPatrolPoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            mover.StartMoveAction(nextPosition, 0.3f);

            /*if (timeSinceArrivedAtWaypoint > waypointDwellTime)
            {
                mover.StartMoveAction(nextPosition);
            }*/
        }

        private bool IsMoveToNextPatrolPoint()
        {
            return Vector3.Distance(transform.position, GetCurrentWaypoint()) < 1f;
        }

        private void UpdateCurrentPatrolPoint()
        {
            currentPatrolPoint = patrolPath.GetNextIndex(currentPatrolPoint);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetPatrolPoint(currentPatrolPoint);
        }
    }
}
