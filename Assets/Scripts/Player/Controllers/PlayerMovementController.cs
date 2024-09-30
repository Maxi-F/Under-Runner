using Events;
using Input;
using MapBounds;
using Player.Controllers;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : PlayerController
    {
        [Header("Input")] [SerializeField] private InputHandlerSO inputHandler;
        [SerializeField] private Vector3EventChannelSO onPlayerNewPositionEvent;

        [Header("MapBounds")]
        [SerializeField] private MapBoundsSO boundsConfig;

        [Header("Movement Config")]
        [SerializeField] private float speed;

        [Header("Look Config")]
        [SerializeField] private Vector2 maxTiltAngles;

        [SerializeField] private GameObject visor;
        [SerializeField] private GameObject bikeBody;

        private Vector3 currentDir;

        private bool _canMove = true;

        public Vector3 CurrentDir => currentDir;

        protected override void OnEnable()
        {
            base.OnEnable();
            inputHandler.onPlayerMove.AddListener(HandleMovement);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerMove.RemoveListener(HandleMovement);
        }

        public void OnUpdate()
        {
            if (_canMove)
            {
                transform.position = boundsConfig.ClampPosition(transform.position + currentDir * (speed * Time.deltaTime), playerCollider.bounds.size);
                onPlayerNewPositionEvent?.RaiseEvent(transform.position);
            }
        }

        private void HandleMovement(Vector2 dir)
        {
            if (currentDir == Vector3.zero && _canMove)
                playerAgent.ChangeStateToMove();
            else if (dir == Vector2.zero && _canMove)
                playerAgent.ChangeStateToIdle();

            currentDir.x = dir.x;
            currentDir.y = 0;
            currentDir.z = dir.y;
        }

        public void TiltAround()
        {
            Vector2 normalizedDir = new Vector2(currentDir.x, currentDir.z).normalized;

            float lateralAngle = Mathf.Asin(normalizedDir.x) * Mathf.Rad2Deg;
            lateralAngle = Mathf.Clamp(lateralAngle, -maxTiltAngles.x, maxTiltAngles.x);

            float frontalAngle = Mathf.Asin(normalizedDir.y) * Mathf.Rad2Deg;
            frontalAngle = Mathf.Clamp(frontalAngle, -maxTiltAngles.y, maxTiltAngles.y);

            bikeBody.transform.rotation = Quaternion.Euler(frontalAngle, 0, -lateralAngle);
        }

        public void ToggleMoveability()
        {
            _canMove = !_canMove;
        }

        public void ToggleMoveability(bool value)
        {
            _canMove = value;
        }
    }
}