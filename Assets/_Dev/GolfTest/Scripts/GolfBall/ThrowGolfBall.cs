using System;
using _Dev.GolfTest.Scripts.InputHandlers;
using UnityEngine;

namespace _Dev.GolfTest.Scripts.GolfBall
{
    public class ThrowGolfBall : MonoBehaviour
    {
        [SerializeField] private InputHandlerSO inputHandlerSo;

        [SerializeField] private float forceMagnitude = 10.0f;
        
        private Vector3 _desiredDirection;

        private bool _shouldThrow = false;
        private Rigidbody _rigidbody;
        
        void Start()
        {
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

        private void FixedUpdate()
        {
            if (_shouldThrow)
            {
                _rigidbody.AddForce(_desiredDirection * forceMagnitude, ForceMode.Impulse);
                _shouldThrow = false;
            }
        }

        void HandleThrow()
        {
            _shouldThrow = true;
        }

        void HandleCameraRotate(Vector2 _lookDelta, bool _isController)
        {
            _desiredDirection = UnityEngine.Camera.main.transform.forward;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, _desiredDirection * 10.0f);
        }
    }
}
