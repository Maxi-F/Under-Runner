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
        [SerializeField] private float spawnCoolDown;
        [SerializeField] private GameObject obstaclePrefab;

        private bool _shouldSpawnObject;
        private Coroutine spawnCoroutine;

        private GameObject _lastSpawnedObstacle = null;
        private bool shouldDisable = false;

        public void OnEnable()
        {
            shouldDisable = false;
            onRoadInstantiatedEvent?.onGameObjectEvent.AddListener(HandleNewRoadInstance);

            StartCoroutine(SpawnObjectCoroutine());
        }

        private void Update()
        {
            if (shouldDisable && _lastSpawnedObstacle == null)
            {
                onObstaclesDisabled.RaiseEvent();
                gameObject.SetActive(false);
            }
        }

        private void OnDisable()
        {
            onRoadInstantiatedEvent?.onGameObjectEvent.RemoveListener(HandleNewRoadInstance);
            if (spawnCoroutine != null)
                StopCoroutine(SpawnObjectCoroutine());
        }

        public void Disable()
        {
            onRoadInstantiatedEvent?.onGameObjectEvent.RemoveListener(HandleNewRoadInstance);
            if (spawnCoroutine != null)
                StopCoroutine(SpawnObjectCoroutine());

            shouldDisable = true;
        }

        private void HandleNewRoadInstance(GameObject road)
        {
            if (!_shouldSpawnObject)
                return;

            Debug.Log("Spawn");

            _shouldSpawnObject = false;
            float roadWidth = road.transform.localScale.x;
            GameObject obstacle = Instantiate(obstaclePrefab, road.transform, false);
            _lastSpawnedObstacle = obstacle;
            obstacle.transform.localPosition = new Vector3(Random.Range(-roadWidth / 2, roadWidth / 2), obstacle.transform.localPosition.y, 0);

            if (spawnCoroutine != null)
                StopCoroutine(SpawnObjectCoroutine());

            spawnCoroutine = StartCoroutine(SpawnObjectCoroutine());
        }

        private IEnumerator SpawnObjectCoroutine()
        {
            Debug.Log("Spawn CoolDown");
            yield return new WaitForSeconds(spawnCoolDown);
            _shouldSpawnObject = true;
        }
    }
}