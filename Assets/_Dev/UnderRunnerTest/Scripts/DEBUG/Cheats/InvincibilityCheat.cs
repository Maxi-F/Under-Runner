using _Dev.UnderRunnerTest.Scripts.DEBUG.Input;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.DEBUG.Cheats
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD || ENABLE_CHEATS

    [RequireComponent(typeof(DebugInputReader))]
    public class InvincibilityCheat : MonoBehaviour
    {
        [SerializeField] private HealthPoints playerHealth;

        private DebugInputReader _debugInputReader;

        private void Start()
        {
            _debugInputReader = GetComponent<DebugInputReader>();

            _debugInputReader.OnInvencibilityCheat += SetInvincibilityCheat;
        }

        private void SetInvincibilityCheat()
        {
            playerHealth.ToggleInvulnerability();
        }
    }
#endif
}