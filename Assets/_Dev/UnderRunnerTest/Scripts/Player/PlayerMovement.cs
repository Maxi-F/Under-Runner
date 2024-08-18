using _Dev.UnderRunnerTest.Scripts.Input;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Input")] [SerializeField] private InputHandlerSO inputHandler;

        [Header("Movement Config")] [SerializeField]
        private float speed;

        [Header("Look Config")] [SerializeField]
        private Vector2 maxLookAngles;

        [SerializeField] private GameObject visor;

        private Vector3 _currentDir;
        private CharacterController _characterController;

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
            _characterController.Move(_currentDir * (speed * Time.deltaTime));
        }

        private void HandleMovement(Vector2 dir)
        {
            _currentDir.x = dir.x;
            _currentDir.y = 0;
            _currentDir.z = dir.y;
            LookAround(dir);
        }

        private void LookAround(Vector2 dir)
        {
            Vector2 normalizedDir = dir.normalized;
            float xAngle = Mathf.Asin(normalizedDir.x) * Mathf.Rad2Deg;
            xAngle = Mathf.Clamp(xAngle, -maxLookAngles.x, maxLookAngles.x);
            visor.transform.rotation = Quaternion.Euler(0, xAngle, 0);
        }
    }
}