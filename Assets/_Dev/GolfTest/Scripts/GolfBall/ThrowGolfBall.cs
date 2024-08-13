using System;
using _Dev.GolfTest.Scripts.Events;
using _Dev.GolfTest.Scripts.GravitySystem;
using _Dev.GolfTest.Scripts.InputHandlers;
using UnityEngine;

namespace _Dev.GolfTest.Scripts.GolfBall
{
    public class ThrowGolfBall : MonoBehaviour
    {
        [Header("Event Channels")] [SerializeField]
        private InputHandlerSO inputHandlerSo;

        [SerializeField] private BoolEventChannelSO canThrowChannelSo;
        [SerializeField] private FloatEventChannelSO powerChangedChannelSo;

        [Header("Throw Properties")] [SerializeField]
        private Vector2 forceMagnitude = new Vector2(1.0f, 10.0f);

        [SerializeField] private float powerScale = 1f;

        [Header("Ball properties")] [SerializeField]
        private float minStopVelocity = 0.01f;

        [SerializeField] private float minCheckVelocity = 0.01f;

        private Vector3 _desiredDirection;

        private bool _isCharging = false;
        private bool _shouldThrow = false;
        private Rigidbody _rigidbody;
        private float _throwPower;

        void Start()
        {
            // TODO put this logic somewhere else, should not be here
            Cursor.lockState = CursorLockMode.Locked;

            _throwPower = forceMagnitude.x;
            _rigidbody ??= GetComponent<Rigidbody>();
        }

        void OnEnable()
        {
            inputHandlerSo.onCameraRotate.AddListener(HandleCameraRotate);
            inputHandlerSo.onThrow.AddListener(HandleChargeThrow);
            inputHandlerSo.onThrowRelease.AddListener(HandleThrow);
        }

        void OnDisable()
        {
            inputHandlerSo.onCameraRotate.RemoveListener(HandleCameraRotate);
            inputHandlerSo.onThrow.RemoveListener(HandleChargeThrow);
            inputHandlerSo.onThrowRelease.RemoveListener(HandleThrow);
        }

        private void Update()
        {
            if (_isCharging)
            {
                _throwPower = Mathf.Clamp(_throwPower + (powerScale * Time.deltaTime), forceMagnitude.x,
                forceMagnitude.y);

                if (Mathf.Approximately(_throwPower, forceMagnitude.y)) _throwPower = forceMagnitude.x;

                powerChangedChannelSo.RaiseEvent(_throwPower);
            }

            if ((_rigidbody.velocity.magnitude >= minCheckVelocity &&
                 _rigidbody.velocity.magnitude <= minStopVelocity) ||
                (TryGetComponent<PhysicalObject>(out PhysicalObject physical) &&
                 physical.CurrentVelocity.magnitude <= minStopVelocity))
            {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
                canThrowChannelSo.RaiseEvent(true);
            }
        }

        private void FixedUpdate()
        {
            if (!_shouldThrow || _isCharging || IsMoving())
            {
                _shouldThrow = false;
                return;
            }

            if (!_isCharging)
            {
                _rigidbody.AddForce(_desiredDirection * _throwPower, ForceMode.Impulse);
                _shouldThrow = false;
                _throwPower = forceMagnitude.x;
            }
        }

        void HandleChargeThrow()
        {
            if (IsMoving()) return;

            Debug.Log("Charging!");
            _isCharging = true;
        }

        void HandleThrow()
        {
            if (!_isCharging) return;

            Debug.Log("Throw!!");
            _shouldThrow = true;
            _isCharging = false;

            canThrowChannelSo.RaiseEvent(false);
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

        private bool IsMoving()
        {
            Debug.Log(_rigidbody.velocity.magnitude);
            return _rigidbody.velocity.magnitude >= 0.1f;
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, _desiredDirection);
        }
    }
}