using _Dev.UnderRunnerTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.Input;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Dev.UnderRunnerTest.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Input")] [SerializeField] private InputHandlerSO inputHandler;
        [SerializeField] private Vector3EventChannelSO onPlayerNewPositionEvent;

        [Header("Movement Config")] [SerializeField]
        private float speed;

        [Header("Look Config")]
        [SerializeField] private Vector2 maxTiltAngles;

        [SerializeField] private GameObject visor;
        [SerializeField] private GameObject bikeBody;
        [SerializeField] private GameObject attackPointPivot;

        private bool _canMove = true;
        private Vector3 currentDir;
        private CharacterController _characterController;
        public Vector3 CurrentDir => currentDir;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        private void OnEnable()
        {
            inputHandler.onPlayerMove.AddListener(HandleMovement);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerMove.RemoveListener(HandleMovement);
        }

        private void Update()
        {
            if (_canMove)
            {
                _characterController.Move(currentDir * (speed * Time.deltaTime));
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