using System;
using Attacks.FallingAttack;
using Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Attacks.FallingBlock
{
    public class FallingAttack : MonoBehaviour
    {
        [SerializeField] private Vector2 initialHeightRange = new Vector2(10.0f, 15.0f);
        [SerializeField] private float heightToDestroy = -1f;
        [SerializeField] private GameObject parentObject;

        [Header("Events")]
        [SerializeField] private GameObjectEventChannelSO onFallingBlockDisabledEvent;

        private Rigidbody _rigidbody;
        private float _acceleration;
        private void Start()
        {
            _rigidbody ??= GetComponent<Rigidbody>();
            
            SetHeight();
        }
        
        private void Update()
        {
            if (transform.position.y < heightToDestroy)
            {
                SetHeight();
                _rigidbody.velocity = Vector3.zero;
                onFallingBlockDisabledEvent?.RaiseEvent(parentObject);
                FallingBlockObjectPool.Instance?.ReturnToPool(parentObject);
            }
        }

        public void SetAcceleration(float newAcceleration)
        {
            _acceleration = newAcceleration;
        }
        
        private void FixedUpdate()
        {
            _rigidbody.AddForce(Vector3.down * _acceleration, ForceMode.Force);
        }

        private void SetHeight()
        {
            Vector3 newPosition = transform.position;
            newPosition.y = Random.Range(initialHeightRange.x, initialHeightRange.y);
            transform.position = newPosition;
        }
    }
}
