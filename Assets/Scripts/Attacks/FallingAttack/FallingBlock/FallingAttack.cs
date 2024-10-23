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

        private float _acceleration;
        private float _velocity;
        private void Start()
        {
            _velocity = 0;
            SetHeight();
        }
        
        private void Update()
        {
            if (transform.position.y < heightToDestroy)
            {
                SetHeight();
                _velocity = 0;
                onFallingBlockDisabledEvent?.RaiseEvent(parentObject);
                FallingBlockObjectPool.Instance?.ReturnToPool(parentObject);
            }
            else
            {
                _velocity += _acceleration * Time.deltaTime;
                transform.position += Vector3.down * (_velocity * Time.deltaTime);
            }
        }

        public void SetAcceleration(float newAcceleration)
        {
            _acceleration = newAcceleration;
        }

        private void SetHeight()
        {
            Vector3 newPosition = transform.position;
            newPosition.y = Random.Range(initialHeightRange.x, initialHeightRange.y);
            transform.position = newPosition;
        }
    }
}
