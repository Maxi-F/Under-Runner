using System;
using System.Collections;
using System.Collections.Generic;
using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;

public class ObstaclesSpawner : MonoBehaviour
{
    [SerializeField] private GameObjectEventChannelSO onRoadInstantiatedEvent;
    [SerializeField] private float spawnCoolDown;
    [SerializeField] private GameObject obstaclePrefab;

    private bool _shouldSpawnObject;
    private Coroutine spawnCoroutine;

    public void Start()
    {
        onRoadInstantiatedEvent?.onGameObjectEvent.AddListener(HandleNewRoadInstance);
    }

    private void Update()
    {
        if (!_shouldSpawnObject)
        {
            if (spawnCoroutine != null)
                StopCoroutine(SpawnObjectCoroutine());

            spawnCoroutine = StartCoroutine(SpawnObjectCoroutine());
        }
    }

    private void OnDestroy()
    {
        onRoadInstantiatedEvent?.onGameObjectEvent.RemoveListener(HandleNewRoadInstance);
    }

    private void HandleNewRoadInstance(GameObject road)
    {
        _shouldSpawnObject = false;
        GameObject obstacle = Instantiate(obstaclePrefab, road.transform, false);
    }

    private IEnumerator SpawnObjectCoroutine()
    {
        yield return new WaitForSeconds(spawnCoolDown);
        _shouldSpawnObject = true;
    }
}