using System;
using System.Collections;
using Events;
using MapBounds;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace ObstacleSystem
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObjectEventChannelSO onRoadInstantiatedEvent;
        [SerializeField] private GameObjectEventChannelSO onRoadDeletedEvent;
        [SerializeField] private GameObjectEventChannelSO onObstacleTriggeredEvent;
        [SerializeField] private VoidEventChannelSO onObstaclesDisabled;
        [SerializeField] private GameObjectEventChannelSO onObstacleDestroyed;
        [SerializeField] private MapBoundsSO mapBounds;
        [SerializeField] private GameObject[] obstaclesPrefabs;

        private bool _shouldSpawnObject;
        private Coroutine _spawnCoroutine;

        private GameObject _lastSpawnedObstacle = null;
        private bool _shouldDisable = false;
        private bool _hasBeenDisabled = false;
        private int _obstaclesCount = 0;

        private float _spawnCoolDown;
        
        public void OnEnable()
        {
            _shouldDisable = false;
            onRoadInstantiatedEvent?.onGameObjectEvent.AddListener(HandleNewRoadInstance);
            onRoadDeletedEvent?.onGameObjectEvent.AddListener(HandleDeleteObstacle);
            onObstacleTriggeredEvent?.onGameObjectEvent.AddListener(HandleDeleteObstacle);
            onObstacleDestroyed?.onGameObjectEvent.AddListener(DeleteObstacle);
        }

        private void Update()
        {
            if (_shouldDisable && _obstaclesCount == 0 && !_hasBeenDisabled)
            {
                onObstaclesDisabled.RaiseEvent();
                _hasBeenDisabled = true;
                if (_spawnCoroutine != null)
                   StopCoroutine(SpawnObjectCoroutine());
            }
        }

        private void OnDisable()
        {
            onRoadInstantiatedEvent?.onGameObjectEvent.RemoveListener(HandleNewRoadInstance);
            onRoadDeletedEvent?.onGameObjectEvent.RemoveListener(HandleDeleteObstacle);
            onObstacleTriggeredEvent?.onGameObjectEvent.RemoveListener(HandleDeleteObstacle);
            onObstacleDestroyed?.onGameObjectEvent.RemoveListener(DeleteObstacle);

            if (_spawnCoroutine != null)
                StopCoroutine(SpawnObjectCoroutine());
        }

        public void Disable()
        {
            onRoadInstantiatedEvent?.onGameObjectEvent.RemoveListener(HandleNewRoadInstance);
            if (_spawnCoroutine != null)
                StopCoroutine(SpawnObjectCoroutine());

            _shouldDisable = true;
        }

        public void StartWithCooldown(float cooldown)
        {
            _spawnCoolDown = cooldown;
            _shouldDisable = false;
            _hasBeenDisabled = false;
            StartCoroutine(SpawnObjectCoroutine());
        }
        
        private void HandleDeleteObstacle(GameObject road)
        {
            ObstaclesCollision obstaclesCollision = road.GetComponentInChildren<ObstaclesCollision>();
            
            if (obstaclesCollision == null) return;
            DeleteObstacle(obstaclesCollision.gameObject);
        }

        private void DeleteObstacle(GameObject obstacle)
        {
            Destroy(obstacle);
            _obstaclesCount--;
        }

        private GameObject GetRandomObstacle()
        {
            return obstaclesPrefabs[Random.Range(0, obstaclesPrefabs.Length)];
        }
        
        private void HandleNewRoadInstance(GameObject road)
        {
            if (!_shouldSpawnObject)
                return;

            _shouldSpawnObject = false;
            float roadWidth = mapBounds.horizontalBounds.max - mapBounds.horizontalBounds.min;
            GameObject obstacle = Instantiate(GetRandomObstacle(), road.transform, false);
            _lastSpawnedObstacle = obstacle;
            obstacle.transform.localPosition = new Vector3(Random.Range(-roadWidth / 2, roadWidth / 2), obstacle.transform.localPosition.y, 0);
            _obstaclesCount++;
            
            if (_spawnCoroutine != null)
                StopCoroutine(SpawnObjectCoroutine());

            _spawnCoroutine = StartCoroutine(SpawnObjectCoroutine());
        }

        private IEnumerator SpawnObjectCoroutine()
        {
            yield return new WaitForSeconds(_spawnCoolDown);
            _shouldSpawnObject = true;
        }
    }
}