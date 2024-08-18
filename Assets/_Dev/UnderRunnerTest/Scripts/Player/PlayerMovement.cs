using System;
using _Dev.UnderRunnerTest.Scripts.Input;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Dev.UnderRunnerTest.Scripts.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Input")] [SerializeField] private InputHandlerSO inputHandler;

        [Header("Movement Config")] [SerializeField]
        private float speed;

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
        }
    }
}