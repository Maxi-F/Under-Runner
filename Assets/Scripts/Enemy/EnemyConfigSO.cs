using UnityEngine;

namespace Enemy
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Enemy/Config", order = 0)]
    public class EnemyConfigSO : ScriptableObject
    {
        public Vector3 defaultPosition;
        public Vector3 weakenedPosition;
    }
}