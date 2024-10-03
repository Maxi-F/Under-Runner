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
        [SerializeField] private MinionsManagerSO minionManagerConfig;
        [SerializeField] private GameObject player;
        
        [Header("Events")]
        [SerializeField] private GameObjectEventChannelSO onMinionDeletedEvent;
        [SerializeField] private VoidEventChannelSO onAllMinionsDestroyedEvent;
        [SerializeField] private VoidEventChannelSO onPlayerDeathEvent;
        [SerializeField] private VoidEventChannelSO onMinionAttackingEvent;
        [SerializeField] private VoidEventChannelSO onMinionAttackedEvent;
        
        public static bool CanAttack { get; private set; }

        private List<GameObject> _minions;
        private bool _isSpawning;
        private Coroutine _spawnCoroutine;
        private int _minionsAttackingCount;
    
        protected void OnEnable()
        {
            _minionsAttackingCount = 0;
            CanAttack = true;
            _spawnCoroutine = StartCoroutine(SpawnMinions());
            onMinionDeletedEvent?.onGameObjectEvent.AddListener(HandleDeletedEvent);
            onPlayerDeathEvent?.onEvent.AddListener(RemoveAllMinions);
            onMinionAttackingEvent?.onEvent.AddListener(AddAttackingMinion);
            onMinionAttackedEvent?.onEvent.AddListener(RemoveAttackingMinion);
        }

        protected void OnDisable()
        {
            onMinionDeletedEvent?.onGameObjectEvent.RemoveListener(HandleDeletedEvent);
            onPlayerDeathEvent?.onEvent.RemoveListener(RemoveAllMinions);
            onMinionAttackingEvent?.onEvent.RemoveListener(AddAttackingMinion);
            onMinionAttackedEvent?.onEvent.RemoveListener(RemoveAttackingMinion);
            StopCoroutine(_spawnCoroutine);
        }

        private void AddAttackingMinion()
        {
            _minionsAttackingCount++;

            Debug.Log($"Add Attacking Minion. Minions Count: {_minionsAttackingCount}");
            if (_minionsAttackingCount >= minionManagerConfig.maxMinionsAttackingAtSameTime)
            {
                CanAttack = false;
            }
        }

        private void RemoveAttackingMinion()
        {
            _minionsAttackingCount--;
            Debug.Log($"Remove Attacking Minion. Minions Count: {_minionsAttackingCount}");

            if (_minionsAttackingCount < minionManagerConfig.maxMinionsAttackingAtSameTime)
            {
                CanAttack = true;
            }
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

            if (_minions.Count == 0 && !_isSpawning)
            {
                onAllMinionsDestroyedEvent?.RaiseEvent();
            }
        }

        private IEnumerator SpawnMinions()
        {
            _isSpawning = true;
            _minions = new List<GameObject>();

            int minionsSpawned = 0;
            while(minionsSpawned < minionSpawnerConfig.minionsToSpawn)
            {
                if(minionsSpawned != 0) yield return new WaitForSeconds(minionSpawnerConfig.timeBetweenSpawns);
                if(_minions.Count >= minionManagerConfig.maxMinionsAtSameTime) continue;
                
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
                minionsSpawned++;
            }

            _isSpawning = false;
            yield return null;
        }
    }
}
