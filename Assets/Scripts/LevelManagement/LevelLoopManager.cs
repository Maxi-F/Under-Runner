using System.Collections;
using Attacks.FallingAttack;
using Events;
using Health;
using Minion.Manager;
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
        [SerializeField] private MinionManager minionManager;
        
        [Header("Spawners")]
        [SerializeField] private ObstaclesSpawner obstaclesSpawner;
        
        [Header("Game Objects")]
        [SerializeField] private GameObject enemy;

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onObstaclesSystemDisabled;
        [SerializeField] private VoidEventChannelSO onAllMinionsDestroyedEvent;
        
        [Header("UI")]
        [SerializeField] private Slider progressBar;
        
        private LevelLoopSO _levelConfig;

        private void Start()
        {
            onObstaclesSystemDisabled.onEvent.AddListener(StartMinionPhase);
            onAllMinionsDestroyedEvent.onEvent.AddListener(StartBossBattle);
        }

        private void OnDisable()
        {
            if (obstaclesSpawner != null)
                onObstaclesSystemDisabled.onEvent.RemoveListener(StartMinionPhase);
            if(minionManager != null)
                onAllMinionsDestroyedEvent.onEvent.RemoveListener(StartBossBattle);
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
            minionManager.gameObject.SetActive(true);
        }

        private void StartBossBattle()
        {
            minionManager.gameObject.SetActive(false);
            enemy.SetActive(true);
        }
        
        public void StartLoopWithConfig(LevelLoopSO loopConfig)
        {
            _levelConfig = loopConfig;
            
            obstaclesSpawner.gameObject.SetActive(false);
            enemy.SetActive(false);
            minionManager.gameObject.SetActive(false);
            fallingBlockSpawner.SetFallingAttackData(_levelConfig.bossData.fallingAttackData);
            
            roadManager.HandleNewVelocity(_levelConfig.roadData.roadVelocity);
            StartCoroutine(ObstaclesCoroutine());
        }
    }
}
