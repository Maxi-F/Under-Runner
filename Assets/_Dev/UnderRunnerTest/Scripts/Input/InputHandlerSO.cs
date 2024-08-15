using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Dev.UnderRunnerTest.Scripts.Input
{
    [CreateAssetMenu(menuName = "Input/InputHandlerSO", fileName = "InputHandler", order = 0)]
    public class InputHandlerSO : ScriptableObject
    {
        public UnityEvent<Vector2> onPlayerMove;

        public void HandleMovement(InputAction.CallbackContext context)
        {
            Vector2 dir = context.ReadValue<Vector2>();

            onPlayerMove?.Invoke(dir);
        }
    }

}