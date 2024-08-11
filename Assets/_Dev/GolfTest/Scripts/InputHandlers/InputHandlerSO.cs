using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace _Dev.GolfTest.Scripts.InputHandlers
{
    [CreateAssetMenu(menuName = "Create Input Handler", fileName = "InputHandlerConfig", order = 0)]
    public class InputHandlerSO : ScriptableObject
    {
        public UnityEvent<Vector2, bool> onCameraRotate;
        public UnityEvent onThrow;

        public void HandleCameraRotate(InputAction.CallbackContext context)
        {
            Vector2 lookInput = context.ReadValue<Vector2>();
            
            onCameraRotate?.Invoke(lookInput, context.control.device != Mouse.current);
        }

        public void HandleThrow(InputAction.CallbackContext context)
        {
            if (context.started)
            {
                onThrow?.Invoke();
            }
        }
    }
}
