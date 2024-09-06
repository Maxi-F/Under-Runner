using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Input
{
    [CreateAssetMenu(menuName = "Input/InputHandlerSO", fileName = "InputHandler", order = 0)]
    public class InputHandlerSO : ScriptableObject
    {
        public UnityEvent<Vector2> onPlayerMove;
        public UnityEvent onPlayerDash;
        public UnityEvent onPlayerAttack;
        
        public UnityEvent onPlayerDashStarted;
        public UnityEvent onPlayerDashFinished;

        public void HandleMovement(InputAction.CallbackContext context)
        {
            Vector2 dir = context.ReadValue<Vector2>();

            onPlayerMove?.Invoke(dir);
        }

        public void HandleDash(InputAction.CallbackContext context)
        {
            // if (context.started)
            //     onPlayerDash?.Invoke();
            
            if(context.started)
                onPlayerDashStarted?.Invoke();
            
            if(context.canceled)
                onPlayerDashFinished?.Invoke();
        }

        public void HandleAttack(InputAction.CallbackContext context)
        {
            if (context.started)
                onPlayerAttack?.Invoke();
        }
    }
}