using System;
using System.Collections;
using System.Collections.Generic;
using _Dev.UnderRunnerTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.Health;
using _Dev.UnderRunnerTest.Scripts.ObstacleSystem;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelLoopManager : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onObstaclesSystemDisabled;
    [SerializeField] private float obstaclesDuration;
    [SerializeField] private ObstaclesSpawner obstaclesSpawner;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject minionEnemy;

    private void Start()
    {
        obstaclesSpawner.gameObject.SetActive(false);
        enemy.SetActive(false);
        minionEnemy.SetActive(false);

        minionEnemy.GetComponent<HealthPoints>().OnDeathEvent.onEvent.AddListener(StartBossBattle);
        onObstaclesSystemDisabled.onEvent.AddListener(StartMinionPhase);
        StartCoroutine(ObstaclesCoroutine());
    }

    private void OnDisable()
    {
        if (minionEnemy != null)
            minionEnemy.GetComponent<HealthPoints>().OnDeathEvent.onEvent.RemoveListener(StartBossBattle);

        if (obstaclesSpawner != null)
            onObstaclesSystemDisabled.onEvent.RemoveListener(StartMinionPhase);
    }

    private IEnumerator ObstaclesCoroutine()
    {
        obstaclesSpawner.gameObject.SetActive(true);
        yield return new WaitForSeconds(obstaclesDuration);
        obstaclesSpawner.Disable();
    }

    private void StartMinionPhase()
    {
        minionEnemy.SetActive(true);
    }

    private void StartBossBattle()
    {
        minionEnemy.SetActive(false);
        enemy.SetActive(true);
    }
}