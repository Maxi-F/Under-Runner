using Events;
using Input;
using MapBounds;
using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
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
        private Collider _playerCollider;

        // private CharacterController _characterController;
        public Vector3 CurrentDir => currentDir;

        private void OnEnable()
        {
            inputHandler.onPlayerMove.AddListener(HandleMovement);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerMove.RemoveListener(HandleMovement);
        }

        private void Awake()
        {
            _playerCollider = GetComponent<Collider>();
        }

        private void Update()
        {
            if (_canMove)
            {
                transform.position = boundsConfig.ClampPosition(transform.position + currentDir * (speed * Time.deltaTime), _playerCollider.bounds.size);
                onPlayerNewPositionEvent?.RaiseEvent(transform.position);
            }
        }

        private void HandleMovement(Vector2 dir)
        {
            currentDir.x = dir.x;
            currentDir.y = 0;
            currentDir.z = dir.y;
            TiltAround(dir);
        }

        private void TiltAround(Vector2 dir)
        {
            Vector2 normalizedDir = dir.normalized;

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