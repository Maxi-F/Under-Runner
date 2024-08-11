using System;
using _Dev.GolfTest.Scripts.InputHandlers;
using UnityEngine;

namespace _Dev.GolfTest.Scripts.Camera
{
    public class CameraHandler : MonoBehaviour
    {
        [SerializeField] private InputHandlerSO _inputHandlerSo;
        
        [Header("Follow Properties")]
        [SerializeField] private Transform target;
        [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, -5);
        [SerializeField] private float followSpeed = 5;

        [Header("Rotation properties")] 
        [Tooltip("Max Angles on the top of the target in which the camera can rotate in X")] 
        [SerializeField] private Vector2 topMinMaxAngles = new Vector2(80.0f, 100.0f);
    
        [Tooltip("Max Angles on the bottom of the target in which the camera can rotate in X")] 
        [SerializeField] private Vector2 bottomMinMaxAngles = new Vector2(260.0f, 280.0f);

        [SerializeField] private float sensibility = 10.0f;
        
        private Vector2 _desiredRotation;
        private bool _isController;
        
        void Start()
        {
            _inputHandlerSo.onCameraRotate.AddListener(HandleCameraRotate);
        }

        private void OnDisable()
        {
            _inputHandlerSo.onCameraRotate.RemoveListener(HandleCameraRotate);
        }

        private void FixedUpdate()
        {
            ModifyPosition();
        }

        private void LateUpdate()
        {
            ModifyRotation();
        }
        
        /// <summary>
        /// Rotates the camera around the target setted as a serialized field,
        /// Taking into account if the rotation was made with controller or mouse.
        ///
        /// It also takes into account top and bottom max angles, so it doesn't do a 360 rotation in X.
        /// </summary>
        private void ModifyRotation()
        {
            Quaternion previousRotationInX = transform.rotation;
            Vector3 previousPositionInX = transform.position;
            float multiplier = Time.deltaTime;
            
            transform.RotateAround(target.position, transform.right, _desiredRotation.y * sensibility * multiplier);

            float angleInX = transform.rotation.eulerAngles.x;
        
            if ((angleInX > topMinMaxAngles.x && angleInX < topMinMaxAngles.y) ||
                (angleInX > bottomMinMaxAngles.x && angleInX < bottomMinMaxAngles.y))
            {
                transform.rotation = previousRotationInX;
                transform.position = previousPositionInX;
            }
        
            transform.RotateAround(target.position, Vector3.up, _desiredRotation.x * sensibility * multiplier);
        }
    
        /// <summary>
        /// Modifies the position of the camera, taking into account the offset
        /// from the target. 
        /// </summary>
        private void ModifyPosition()
        {
            var rotatedOffset = transform.rotation * offset;
            var offsetEmulatingTransformPoint = target.position + rotatedOffset;

            transform.position = Vector3.Slerp(transform.position, offsetEmulatingTransformPoint, Time.fixedDeltaTime * followSpeed);
        }

        void HandleCameraRotate(Vector2 direction, bool isController)
        {
            _isController = isController;

            Vector2 lookInputToUse = direction;

            if (isController)
            {
                _desiredRotation = lookInputToUse;
            } else
            {
                _desiredRotation = new Vector2(lookInputToUse.x, -lookInputToUse.y);
            }
        }
    }
}
