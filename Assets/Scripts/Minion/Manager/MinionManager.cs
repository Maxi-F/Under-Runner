using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Events;
using Minion.ScriptableObjects;
using UnityEngine;

namespace Minion.Manager
{
    public class MinionManager : MonoBehaviour
    {
        [SerializeField] private MinionSpawnerSO minionSpawnerConfig;
        [SerializeField] private GameObject player;
        
        [Header("Events")]
        [SerializeField] private GameObjectEventChannelSO onMinionDeletedEvent;
        [SerializeField] private VoidEventChannelSO onAllMinionsDestroyedEvent;
        [SerializeField] private VoidEventChannelSO onPlayerDeathEvent;
        
        private List<GameObject> _minions;
        private bool _isSpawning;
        private Coroutine _spawnCoroutine;
    
        protected void OnEnable()
        {
            _spawnCoroutine = StartCoroutine(SpawnMinions());
            onMinionDeletedEvent?.onGameObjectEvent.AddListener(HandleDeletedEvent);
            onPlayerDeathEvent?.onEvent.AddListener(RemoveAllMinions);
        }

        protected void OnDisable()
        {
            onMinionDeletedEvent?.onGameObjectEvent.RemoveListener(HandleDeletedEvent);
            onPlayerDeathEvent?.onEvent.RemoveListener(RemoveAllMinions);
            StopCoroutine(_spawnCoroutine);
        }

        private void RemoveAllMinions()
        {
            foreach (var minion in _minions.ToList())
            {
                _minions.Remove(minion);
                MinionObjectPool.Instance?.ReturnToPool(minion);
            }
        }

        private void HandleDeletedEvent(GameObject deletedMinion)
        {
            MinionObjectPool.Instance?.ReturnToPool(deletedMinion);
            
            _minions.Remove(deletedMinion);

            Debug.Log($"MINIONS COUNT: {_minions.Count}, IS SPAWNING: {_isSpawning}");
            if (_minions.Count == 0 && !_isSpawning)
            {
                onAllMinionsDestroyedEvent?.RaiseEvent();
            }
        }

        private IEnumerator SpawnMinions()
        {
            _isSpawning = true;
            _minions = new List<GameObject>();

            for (int i = 0; i < minionSpawnerConfig.minionsToSpawn; i++)
            {
                if(i != 0) yield return new WaitForSeconds(minionSpawnerConfig.timeBetweenSpawns);
                
                GameObject minion = MinionObjectPool.Instance?.GetPooledObject();
                if (minion == null)
                {
                    Debug.LogError("Minion was null");
                    break;
                }

                MinionAgent minionAgent = minion.GetComponent<MinionAgent>();
                minionAgent.SetPlayer(player);
                
                minion.transform.position = minionSpawnerConfig.GetSpawnPoint();
                minion.SetActive(true);

                _minions.Add(minion);
            }

            _isSpawning = false;
            yield return null;
        }
    }
}
