using _Dev.UnderRunnerTest.Scripts.DEBUG.Input;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.DEBUG.Cheats
{
#if UNITY_EDITOR || DEVELOPMENT_BUILD || ENABLE_CHEATS

    public class InvincibilityCheat : MonoBehaviour
    {
        [SerializeField] private CheatsConfigSO _cheatsConfigSo;

        public void SetInvincibilityCheat()
        {
            _cheatsConfigSo.playerHealth.ToggleInvulnerability();
        }
    }
#endif
}