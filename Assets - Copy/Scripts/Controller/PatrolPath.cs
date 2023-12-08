using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Controller
{
    public class PatrolPath : MonoBehaviour
    {
        const float patrolWayPointRadius = 0.6f;

        private void OnDrawGizmos()
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                int j = GetNextIndex(i);
                Gizmos.DrawSphere(GetPatrolPoint(i), patrolWayPointRadius);
                Gizmos.DrawLine(GetPatrolPoint(i), GetPatrolPoint(j));
            }
        }

        public int GetNextIndex(int i)
        {
            if (i + 1 == transform.childCount)
            {
                return 0;
            }
            return i + 1;
        }

        public Vector3 GetPatrolPoint(int i)
        {
            return transform.GetChild(i).position;
        }
    }
}
