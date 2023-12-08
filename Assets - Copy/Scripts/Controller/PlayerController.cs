using RPG.Action.Combat;
using RPG.Action.Movement;
using RPG.Stats.CharacterStats;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Controller
{
    public class PlayerController : MonoBehaviour
    {
        private int clickCount;
        public bool isResetClickCountRunning;

        // Start is called before the first frame update
        void Start()
        {
           
        }

        // Update is called once per frame
        void Update()
        {
            if (GetComponent<Health>().IsDead()) return;

            if (CombatInteract()) return;
            if (MovementInteract()) return;

        }

        private bool MovementInteract()
        {
            RaycastHit rayHit;
            bool isRayHit = Physics.Raycast(GetMouseRay(), out rayHit);

            if (isRayHit)
            {
                if (Input.GetMouseButtonDown(0) && clickCount == 0)
                {
                    clickCount++;
                    GetComponent<Mover>().StartMoveAction(rayHit.point, 0.3f);
                }
                else if (Input.GetMouseButtonDown(0) && clickCount > 0)
                {
                    StopCoroutine(ResetClickCount());
                    GetComponent<Mover>().StartMoveAction(rayHit.point, 1);
                }
                StartCoroutine(ResetClickCount());

                return true;
            }

            return false;
        }

        IEnumerator ResetClickCount()
        {
            if (!isResetClickCountRunning)
            {
                isResetClickCountRunning = true;
                yield return new WaitForSeconds(1);
                clickCount = 0;
                isResetClickCountRunning = false;
            }
        }

        private bool CombatInteract()
        {
            RaycastHit[] rayHits = Physics.RaycastAll(GetMouseRay());

            foreach (var hit in rayHits)
            {
                var combatTarget = hit.transform.GetComponent<CombatTarget>();

                if (combatTarget == null || !GetComponent<Fighter>().IsAttackable(combatTarget.gameObject)) continue;

                if (Input.GetMouseButton(0)) 
                {
                    GetComponent<Fighter>().StartAttackAction(combatTarget.gameObject);
                }

                return true;
            }

            return false;
        }

        Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
