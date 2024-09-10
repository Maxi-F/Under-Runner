using ObjectPool.Runtime;
using Roads.ScriptableObjects;
using UnityEngine;

namespace Roads
{
    public class RoadFactory : IFactory<RoadsConfigSO>
    {
        private RoadsConfigSO _config;
        private int _index = 0;
        
        public void SetConfig(RoadsConfigSO config)
        {
            _config = config;
        }

        public GameObject CreateObject()
        {
            GameObject newLastRoad = Object.Instantiate(_config.roads[_index].roadSection);
            newLastRoad.transform.rotation = _config.roads[_index].startRotation;

            _index++;
            if (_index >= _config.roads.Count) _index = 0;
            
            return newLastRoad;
        }
    }
}
