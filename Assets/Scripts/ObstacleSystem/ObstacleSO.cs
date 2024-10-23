using UnityEngine;

namespace ObstacleSystem
{
    [CreateAssetMenu(menuName = "Obstacles/config")]
    public class ObstacleSO : ScriptableObject
    {
        public GameObject[] prefabs;
    }
}