using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.Health;
using _Dev.UnderRunnerTest.Scripts.ObstacleSystem;
using UnityEngine;
using UnityEngine.UI;

public class LevelLoopManager : MonoBehaviour
{
    [SerializeField] private VoidEventChannelSO onObstaclesSystemDisabled;
    [SerializeField] private float obstaclesDuration;
    [SerializeField] private ObstaclesSpawner obstaclesSpawner;
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject enemyHealthBar;
    [SerializeField] private GameObject minionEnemy;

    [Header("UI")]
    [SerializeField] private Slider progressBar;

    private void Start()
    {
        obstaclesSpawner.gameObject.SetActive(false);
        enemy.SetActive(false);
        enemyHealthBar.SetActive(false);
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
        float timer = 0;
        float startTime = Time.time;

        obstaclesSpawner.gameObject.SetActive(true);
        while (timer < obstaclesDuration)
        {
            timer = Time.time - startTime;
            progressBar.value = Mathf.Lerp(0, progressBar.maxValue, timer / obstaclesDuration);
            yield return null;
        }

        progressBar.gameObject.SetActive(false);

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
        enemyHealthBar.SetActive(true);
    }
}