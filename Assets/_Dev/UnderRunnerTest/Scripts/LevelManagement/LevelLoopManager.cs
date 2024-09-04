using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.Health;
using _Dev.UnderRunnerTest.Scripts.ObstacleSystem;
using _Dev.UnderRunnerTest.Scripts.Roads;
using UnityEngine;
using UnityEngine.UI;

namespace _Dev.UnderRunnerTest.Scripts.LevelManagement
{
    public class LevelLoopManager : MonoBehaviour
    {
        [Header("Level Config")]
        [SerializeField] private LevelLoopSO levelConfig;

        [Header("Managers")] [SerializeField] private RoadManager roadManager;
        
        [Header("Spawners")]
        [SerializeField] private ObstaclesSpawner obstaclesSpawner;
        
        [Header("Game Objects")]
        [SerializeField] private GameObject enemy;
        [SerializeField] private GameObject enemyHealthBar;
        [SerializeField] private GameObject minionEnemy;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onObstaclesSystemDisabled;
        
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

            roadManager.HandleNewVelocity(levelConfig.roadData.roadVelocity);

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
            float obstaclesDuration = levelConfig.obstacleData.obstaclesDuration;
            float obstacleCooldown = levelConfig.obstacleData.obstacleCooldown;
            float startTime = Time.time;

            obstaclesSpawner.gameObject.SetActive(true);
            obstaclesSpawner.StartWithCooldown(obstacleCooldown);
            
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
}
