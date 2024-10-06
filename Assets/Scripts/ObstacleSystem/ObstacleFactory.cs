using ObjectPool.Runtime;
using UnityEngine;

namespace ObstacleSystem
{
    public class ObstacleFactory : IFactory<ObstacleSO>
    {
        private ObstacleSO _config;
        
        public void SetConfig(ObstacleSO config)
        {
            _config = config;
        }

        public GameObject CreateObject()
        {
            return GameObject.Instantiate(_config.prefab);
        }
    }
}