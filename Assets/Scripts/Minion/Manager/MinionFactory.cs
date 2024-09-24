using Minion.ScriptableObjects;
using ObjectPool.Runtime;
using UnityEngine;

namespace Minion.Manager
{
    public class MinionFactory : IFactory<MinionSO>
    {
        private MinionSO _config;

        public void SetConfig(MinionSO config)
        {
            _config = config;
        }

        public GameObject CreateObject()
        {
            GameObject minionObject = GameObject.Instantiate(_config.minionPrefab);
            return minionObject;
        }
    }
}
