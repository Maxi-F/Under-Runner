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

        [Header("Look Config")] [SerializeField]
        private Vector2 maxLookAngles;

        [SerializeField] private GameObject visor;
        [SerializeField] private GameObject attackPointPivot;

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
            _characterController.Move(currentDir * (speed * Time.deltaTime));
            onPlayerNewPositionEvent?.RaiseEvent(transform.position);
        }

        private void HandleMovement(Vector2 dir)
        {
            currentDir.x = dir.x;
            currentDir.y = 0;
            currentDir.z = dir.y;
            LookAround(dir);
        }

        private void LookAround(Vector2 dir)
        {
            Vector2 normalizedDir = dir.normalized;
            float xAngle = Mathf.Asin(normalizedDir.x) * Mathf.Rad2Deg;
            xAngle = Mathf.Clamp(xAngle, -maxLookAngles.x, maxLookAngles.x);

            float yAngle = normalizedDir.y < 0 ? Mathf.Asin(normalizedDir.y) * Mathf.Rad2Deg : 0;
            yAngle = Mathf.Clamp(yAngle, -maxLookAngles.y, maxLookAngles.y);

            visor.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0);
            attackPointPivot.transform.rotation = Quaternion.Euler(yAngle, xAngle, 0);
        }
    }
}