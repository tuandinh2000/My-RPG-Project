using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Action.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField]
        private float maxSpeed;
        private ActionSchedular actionSchedular;
        private NavMeshAgent _navMeshAgent;
        private int _forwardSpeedHash;

        // Start is called before the first frame update
        void Start()
        {
            actionSchedular = GetComponent<ActionSchedular>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _forwardSpeedHash = Animator.StringToHash("forwardSpeed");

        }

        // Update is called once per frame
        void Update()
        {
            UpdateAnimation();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            actionSchedular.StartAction(this);
            MoveToDestination(destination, speedFraction);
        }

        public void CancelAction()
        {
            _navMeshAgent.isStopped = true;
        }

        public void MoveToDestination(Vector3 destination, float speedFraction)
        {
            _navMeshAgent.destination = destination;
            _navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction); 
            _navMeshAgent.isStopped = false;
        }

        void UpdateAnimation()
        {
            Vector3 globalVelocity = GetComponent<NavMeshAgent>().velocity;
            Vector3 localVelocity = transform.InverseTransformVector(globalVelocity);

            GetComponent<Animator>().SetFloat(_forwardSpeedHash, localVelocity.z);
        }
    }
}
