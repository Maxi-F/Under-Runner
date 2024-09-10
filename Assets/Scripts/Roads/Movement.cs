using System;
using Events;
using UnityEngine;

namespace Roads
{
    public class Movement : MonoBehaviour
    {
        [SerializeField] private Vector3 velocity;
        [SerializeField] private Transform road;
        [SerializeField] private Vector3EventChannelSO onNewVelocity;

        private Vector3 _velocityToUse;
        
        private void OnEnable()
        {
            _velocityToUse = velocity;
            onNewVelocity?.onVectorEvent.AddListener(SetVelocity);
        }

        private void OnDisable()
        {
            onNewVelocity?.onVectorEvent.RemoveListener(SetVelocity);
        }

        public void SetVelocity(Vector3 newVelocity)
        {
            _velocityToUse = newVelocity;
        }

        private void Update()
        {
            road.position += _velocityToUse * Time.deltaTime;
        }
    }
}
