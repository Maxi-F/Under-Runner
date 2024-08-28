using System;
using System.Collections;
using System.Collections.Generic;
using _Dev.UnderRunnerTest.Scripts.Health;
using _Dev.UnderRunnerTest.Scripts.ObstacleSystem;
using Microsoft.Win32.SafeHandles;
using UnityEngine;
using UnityEngine.Serialization;

public class LevelLoopManager : MonoBehaviour
{
    [SerializeField] private float obstaclesDuration;
    [SerializeField] private GameObject obstaclesSpawner;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject minionEnemy;

    private void Start()
    {
        obstaclesSpawner.SetActive(false);
        enemy.SetActive(false);
        minionEnemy.SetActive(false);

        minionEnemy.GetComponent<HealthPoints>().OnDeathEvent.onEvent.AddListener(StartBossBattle);

        StartCoroutine(ObstaclesCoroutine());
    }

    private void OnDisable()
    {
        if (minionEnemy != null)
            minionEnemy.GetComponent<HealthPoints>().OnDeathEvent.onEvent.RemoveListener(StartBossBattle);
    }

    private IEnumerator ObstaclesCoroutine()
    {
        obstaclesSpawner.SetActive(true);
        yield return new WaitForSeconds(obstaclesDuration);
        obstaclesSpawner.SetActive(false);
        minionEnemy.SetActive(true);
    }

    private void StartBossBattle()
    {
        minionEnemy.SetActive(false);
        enemy.SetActive(true);
    }
}