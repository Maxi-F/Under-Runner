using System.Collections;
using Events;
using Input;
using Managers;
using MapBounds;
using Player.Controllers;
using UnityEngine;

namespace Player
{
    public class PlayerMovementController : PlayerController
    {
        [SerializeField] private PauseSO pauseData;

        [Header("Input")] [SerializeField] private InputHandlerSO inputHandler;
        [SerializeField] private Vector3EventChannelSO onPlayerNewPositionEvent;
        [SerializeField] private Vector3EventChannelSO onPlayerMovementEvent;
        [SerializeField] private VoidEventChannelSO onCinematicStarted;
        [SerializeField] private VoidEventChannelSO onCinematicFinished;
        
        [Header("MapBounds")]
        [SerializeField] private MapBoundsSO boundsConfig;

        [Header("Movement Config")]
        [SerializeField] private float speed;

        private Vector3 _currentDir;
        private Coroutine _handleTiltCoroutine;
        private bool _canMove = true;
        private bool _isInCinematic = false;

        public Vector3 CurrentDir => _currentDir;

        protected override void OnEnable()
        {
            base.OnEnable();
            onCinematicStarted?.onEvent.AddListener(HandleCinematic);
            onCinematicFinished?.onEvent.AddListener(HandleEndCinematic);
            inputHandler.onPlayerMove.AddListener(HandleMovement);
        }

        private void OnDisable()
        {
            onCinematicStarted?.onEvent.RemoveListener(HandleCinematic);
            onCinematicFinished?.onEvent.RemoveListener(HandleEndCinematic);
            inputHandler.onPlayerMove.RemoveListener(HandleMovement);
        }

        public void OnUpdate()
        {
            if (_canMove && !_isInCinematic)
            {
                Vector3 previousPosition = transform.position;
                transform.position = boundsConfig.ClampPosition(transform.position + _currentDir * (speed * Time.deltaTime), playerCollider.bounds.size);
                onPlayerNewPositionEvent?.RaiseEvent(transform.position);

                onPlayerMovementEvent?.RaiseEvent(transform.position - previousPosition);
            }
        }
        
        private void HandleEndCinematic()
        {
            _isInCinematic = false;
        }

        private void HandleCinematic()
        {
            _isInCinematic = true;
        }

        private void HandleMovement(Vector2 dir)
        {
            if (_currentDir == Vector3.zero && _canMove)
                playerAgent.ChangeStateToMove();
            else if (dir == Vector2.zero && _canMove)
                playerAgent.ChangeStateToIdle();

            _currentDir.x = dir.x;
            _currentDir.y = 0;
            _currentDir.z = dir.y;
        }

        public void TiltAround()
        {
            Vector2 normalizedDir = new Vector2(_currentDir.x, _currentDir.z).normalized;
            animatorHandler.SetPlayerDirection(normalizedDir);
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