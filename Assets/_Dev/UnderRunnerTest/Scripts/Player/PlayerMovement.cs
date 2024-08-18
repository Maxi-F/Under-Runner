using _Dev.UnderRunnerTest.Scripts.Input;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Player
{
    public class PlayerMovement : MonoBehaviour
    {
        [Header("Input")] 
        [SerializeField] private InputHandlerSO inputHandler;

        [Header("Movement Config")] 
        [SerializeField] private float speed;

        private Vector3 _currentDir;

        private void OnEnable()
        {
            inputHandler.onPlayerMove.AddListener(HandleMovement);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerMove.RemoveListener(HandleMovement);
        }

        // Update is called once per frame
        private void Update()
        {
            transform.Translate(_currentDir * (speed * Time.deltaTime));
        }

        private void HandleMovement(Vector2 dir)
        {
            _currentDir.x = dir.x;
            _currentDir.y = 0;
            _currentDir.z = dir.y;
        }
    }
}