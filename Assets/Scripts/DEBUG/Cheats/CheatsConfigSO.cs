using Health;
using UnityEngine;

namespace DEBUG.Cheats
{
    [CreateAssetMenu(fileName = "CheatsConfig", menuName = "Debug/Cheats/Config", order = 0)]
    public class CheatsConfigSO : ScriptableObject
    {
        public HealthPoints playerHealth;
    }
}
