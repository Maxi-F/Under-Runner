using System;
using _Dev.GolfTest.Scripts.InputHandlers;
using UnityEngine;

namespace _Dev.GolfTest.Scripts.GolfBall
{
    public class ThrowGolfBall : MonoBehaviour
    {
        [SerializeField] private InputHandlerSO inputHandlerSo;

        [Header("Throw Properties")]
        [SerializeField] private float forceMagnitude = 10.0f;

        [Header("Ball properties")] 
        [SerializeField] private float minStopVelocity = 0.01f;

        [SerializeField] private float minCheckVelocity = 0.01f;
        
        private Vector3 _desiredDirection;

        private bool _shouldThrow = false;
        private Rigidbody _rigidbody;
        
        void Start()
        {
            // TODO put this logic somewhere else, should not be here
            Cursor.lockState = CursorLockMode.Locked;
            
            _rigidbody ??= GetComponent<Rigidbody>();
        }
        
        void OnEnable()
        {
            inputHandlerSo.onCameraRotate.AddListener(HandleCameraRotate);
            inputHandlerSo.onThrow.AddListener(HandleThrow);

        }

        void OnDisable()
        {
            inputHandlerSo.onCameraRotate.RemoveListener(HandleCameraRotate);
            inputHandlerSo.onThrow.RemoveListener(HandleThrow);
        }

        private void Update()
        {
            if (_rigidbody.velocity.magnitude >= minCheckVelocity && 
                _rigidbody.velocity.magnitude <= minStopVelocity)
            {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
            }
        }
        
        private void FixedUpdate()
        {
            if (!_shouldThrow || _rigidbody.velocity.magnitude >= float.Epsilon)
            {
                _shouldThrow = false;
                return;
            }
            
            _rigidbody.AddForce(_desiredDirection * forceMagnitude, ForceMode.Impulse);
            _shouldThrow = false;
        }

        void HandleThrow()
        {
            _shouldThrow = true;
        }

        void HandleCameraRotate(Vector2 _lookDelta, bool _isController)
        {
            if (UnityEngine.Camera.main == null)
            {
                Debug.LogError("Camera main is null!");
                return;
            }
            _desiredDirection = UnityEngine.Camera.main.transform.forward;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, _desiredDirection);
        }
    }
}
