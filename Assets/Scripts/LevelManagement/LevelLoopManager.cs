using System.Collections;
using ObstacleSystem;
using UnityEngine;

namespace LevelManagement
{
    public class LevelLoopManager : MonoBehaviour
    {
        [Header("Spawners")]
        [SerializeField] private ObstaclesSpawner obstaclesSpawner;

        [Header("Sequences")]
        [SerializeField] private ObstacleSequence obstacleSequence;
        [SerializeField] private MinionsSequence minionsSequence;
        [SerializeField] private BossSequence bossSequence;
        
        private LevelLoopSO _levelConfig;

        public void StartLevelSequence(LevelLoopSO loopConfig)
        {
            SetupLevelLoop(loopConfig);
            StartCoroutine(StartLoopWithConfig());
        }

        private void SetupLevelLoop(LevelLoopSO loopConfig)
        {
            _levelConfig = loopConfig;

            obstacleSequence.SetupSequence(_levelConfig.roadData);
            minionsSequence.SetupSequence();
            bossSequence.SetupSequence(_levelConfig.bossData);
            
            obstacleSequence.SetLevelConfig(_levelConfig);
            minionsSequence.SetPostAction(bossSequence.StartBossBattle());
            obstacleSequence.SetPostAction(minionsSequence.StartMinionPhase());
        }

        private IEnumerator StartLoopWithConfig()
        {
            return obstacleSequence.Execute();
        }
    }
}
