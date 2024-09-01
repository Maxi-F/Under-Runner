using System;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Dev.UnderRunnerTest.Scripts.DEBUG.Input
{
    public class DebugInputReader : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD || ENABLE_CHEATS
        private DebugActions _debugInput;

        public event Action OnInvencibilityCheat;
        
        private void Start()
        {
            _debugInput = new DebugActions();
            _debugInput.Enable();

            _debugInput.Cheats.Invencibility.performed += HandleInvencibilityCheat;
        }

        private void OnDestroy()
        {
            _debugInput.Cheats.Invencibility.performed -= HandleInvencibilityCheat;
        }

        private void HandleInvencibilityCheat(InputAction.CallbackContext context)
        {
            OnInvencibilityCheat?.Invoke();
        }

#endif
    }
}