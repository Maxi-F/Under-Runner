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
using Utils;

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

        [Header("Sequences")]
        [SerializeField] private ObstacleSequence obstacleSequence;
        [SerializeField] private MinionSequence minionSequence;
        
        private LevelLoopSO _levelConfig;

        private IEnumerator BossBattleAction()
        {
            yield return null;
            enemy.SetActive(true);
        }

        private IEnumerator StartBossBattle()
        {
            Sequence sequence = new Sequence();

            sequence.SetAction(BossBattleAction());

            return sequence.Execute();
        }

        public void StartLevelSequence(LevelLoopSO loopConfig)
        {
            SetupLevelLoop(loopConfig);
            StartCoroutine(StartLoopWithConfig());
        }

        private void SetupLevelLoop(LevelLoopSO loopConfig)
        {
            _levelConfig = loopConfig;

            obstacleSequence.SetLevelConfig(_levelConfig);
            minionSequence.SetPostAction(StartBossBattle());
            obstacleSequence.SetPostAction(minionSequence.StartMinionPhase());

            obstaclesSpawner.gameObject.SetActive(false);
            enemy.SetActive(false);
            minionManager.gameObject.SetActive(false);
            fallingBlockSpawner.SetFallingAttackData(_levelConfig.bossData.fallingAttackData);

            roadManager.HandleNewVelocity(_levelConfig.roadData.roadVelocity);
        }

        private IEnumerator StartLoopWithConfig()
        {
            return obstacleSequence.Execute();
        }
    }
}
