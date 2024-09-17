using System.Collections;
using Attacks.FallingAttack;
using Events;
using Health;
using ObstacleSystem;
using Roads;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace LevelManagement
{
    public class LevelLoopManager : MonoBehaviour
    {
        [Header("Managers")] 
        [SerializeField] private RoadManager roadManager;
        [SerializeField] private FallingBlockSpawner fallingBlockSpawner;
        
        [Header("Spawners")]
        [SerializeField] private ObstaclesSpawner obstaclesSpawner;
        
        [Header("Game Objects")]
        [SerializeField] private GameObject enemy;
        [SerializeField] private GameObject minionEnemy;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onObstaclesSystemDisabled;
        
        [Header("UI")]
        [SerializeField] private Slider progressBar;
        
        private LevelLoopSO _levelConfig;

        private void Start()
        {
            onObstaclesSystemDisabled.onEvent.AddListener(StartMinionPhase);
            minionEnemy.GetComponent<HealthPoints>().OnDeathEvent.onEvent.AddListener(StartBossBattle);
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
            float obstaclesDuration = _levelConfig.obstacleData.obstaclesDuration;
            float obstacleCooldown = _levelConfig.obstacleData.obstacleCooldown;
            float startTime = Time.time;

            obstaclesSpawner.gameObject.SetActive(true);
            progressBar.gameObject.SetActive(true);

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
        }
        
        public void StartLoopWithConfig(LevelLoopSO loopConfig)
        {
            _levelConfig = loopConfig;
            
            obstaclesSpawner.gameObject.SetActive(false);
            enemy.SetActive(false);
            minionEnemy.SetActive(false);
            fallingBlockSpawner.SetFallingAttackData(_levelConfig.bossData.fallingAttackData);
            
            roadManager.HandleNewVelocity(_levelConfig.roadData.roadVelocity);
            StartCoroutine(ObstaclesCoroutine());
        }
    }
}
