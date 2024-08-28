using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.ObstacleSystem
{
    public class ObstaclesSpawner : MonoBehaviour
    {
        [SerializeField] private GameObjectEventChannelSO onRoadInstantiatedEvent;
        [SerializeField] private float spawnCoolDown;
        [SerializeField] private GameObject obstaclePrefab;

        private bool _shouldSpawnObject;
        private Coroutine spawnCoroutine;

        public void OnEnable()
        {
            onRoadInstantiatedEvent?.onGameObjectEvent.AddListener(HandleNewRoadInstance);

            StartCoroutine(SpawnObjectCoroutine());
        }

        private void OnDisable()
        {
            onRoadInstantiatedEvent?.onGameObjectEvent.RemoveListener(HandleNewRoadInstance);
            if (spawnCoroutine != null)
                StopCoroutine(SpawnObjectCoroutine());
        }

        private void HandleNewRoadInstance(GameObject road)
        {
            if (!_shouldSpawnObject)
                return;

            Debug.Log("Spawn");

            _shouldSpawnObject = false;
            float roadWidth = road.transform.localScale.x;
            GameObject obstacle = Instantiate(obstaclePrefab, road.transform, false);

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