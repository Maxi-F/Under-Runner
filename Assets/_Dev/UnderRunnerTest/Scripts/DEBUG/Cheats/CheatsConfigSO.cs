using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.DEBUG.Cheats
{
    [CreateAssetMenu(fileName = "CheatsConfig", menuName = "Debug/Cheats/Config", order = 0)]
    public class CheatsConfigSO : ScriptableObject
    {
        public HealthPoints playerHealth;
    }
}
