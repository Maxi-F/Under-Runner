using System;
using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Dev.UnderRunnerTest.Scripts.ObstacleSystem
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObjectEventChannelSO onRoadInstantiatedEvent;
        [SerializeField] private VoidEventChannelSO onObstaclesDisabled;
        [SerializeField] private GameObject obstaclePrefab;

        private bool _shouldSpawnObject;
        private Coroutine _spawnCoroutine;

        private GameObject _lastSpawnedObstacle = null;
        private bool _shouldDisable = false;

        private float _spawnCoolDown;
        
        public void OnEnable()
        {
            _shouldDisable = false;
            onRoadInstantiatedEvent?.onGameObjectEvent.AddListener(HandleNewRoadInstance);
        }

        private void Update()
        {
            if (_shouldDisable && _lastSpawnedObstacle == null)
            {
                onObstaclesDisabled.RaiseEvent();
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            onRoadInstantiatedEvent?.onGameObjectEvent.RemoveListener(HandleNewRoadInstance);
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
            StartCoroutine(SpawnObjectCoroutine());
        }

        private void HandleNewRoadInstance(GameObject road)
        {
            if (!_shouldSpawnObject)
                return;

            _shouldSpawnObject = false;
            float roadWidth = road.transform.localScale.x;
            GameObject obstacle = Instantiate(obstaclePrefab, road.transform, false);
            _lastSpawnedObstacle = obstacle;
            obstacle.transform.localPosition = new Vector3(Random.Range(-roadWidth / 2, roadWidth / 2), obstacle.transform.localPosition.y, 0);

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