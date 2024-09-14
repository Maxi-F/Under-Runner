using System;
using Attacks.FallingAttack;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Attacks.FallingBlock
{
    public class FallingBlock : MonoBehaviour
    {
        [SerializeField] private Vector2 initialHeightRange = new Vector2(10.0f, 15.0f);
        [SerializeField] private float heightToDestroy = -1f;
        [SerializeField] private GameObject parentObject;
        [SerializeField] private float acceleration = 10.0f;

        private Rigidbody _rigidbody;
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
                FallingBlockObjectPool.Instance?.ReturnToPool(parentObject);
            }
        }

        private void FixedUpdate()
        {
            _rigidbody.AddForce(Vector3.down * acceleration, ForceMode.Force);
        }

        private void SetHeight()
        {
            Vector3 newPosition = transform.position;
            newPosition.y = Random.Range(initialHeightRange.x, initialHeightRange.y);
            transform.position = newPosition;
        }
    }
}
