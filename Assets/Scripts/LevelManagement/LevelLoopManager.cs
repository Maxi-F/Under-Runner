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

        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onObstaclesSystemDisabled;
        [SerializeField] private VoidEventChannelSO onAllMinionsDestroyedEvent;

        [Header("Sequences")]
        [SerializeField] private ObstacleSequence obstacleSequence;
        
        private LevelLoopSO _levelConfig;
        private bool _areAllMinionsDestroyed;

        private void Start()
        {
            onObstaclesSystemDisabled.onEvent.AddListener(HandleObstacleSystemDisabled);
            onAllMinionsDestroyedEvent.onEvent.AddListener(HandleAllMinionsDestroyed);
        }

        private void OnDisable()
        {
            if (obstaclesSpawner != null)
                onObstaclesSystemDisabled.onEvent.RemoveListener(HandleObstacleSystemDisabled);
            if(minionManager != null)
                onAllMinionsDestroyedEvent.onEvent.RemoveListener(HandleAllMinionsDestroyed);
        }

        private void HandleAllMinionsDestroyed()
        {
            _areAllMinionsDestroyed = true;
        }

        private IEnumerator SetMinionManager(bool value)
        {
            minionManager.gameObject.SetActive(value);

            yield return new WaitUntil(() => _areAllMinionsDestroyed);
        }

        private IEnumerator MinionSequencePreActions()
        {
            _areAllMinionsDestroyed = false;

            yield return null;
        }

        private IEnumerator StartMinionPhase()
        {
            Sequence minionSequence = new Sequence();

            minionSequence.AddPreAction(MinionSequencePreActions());
            minionSequence.SetAction(SetMinionManager(true));
            minionSequence.AddPostAction(SetMinionManager(false));
            minionSequence.AddPostAction(StartBossBattle());

            return minionSequence.Execute();
        }

        private void HandleObstacleSystemDisabled()
        {
            obstacleSequence.SetObstacleSequenceAsDisabled();
        }

        private IEnumerator BossBattleAction()
        {
            enemy.SetActive(true);
            yield return null;
        }

        private IEnumerator StartBossBattle()
        {
            Sequence sequence = new Sequence();

            sequence.SetAction(BossBattleAction());

            return sequence.Execute();
        }

        private IEnumerator LevelCoroutine(Sequence sequence)
        {
            return sequence.Execute();
        }

        public void StartLevelSequence(LevelLoopSO loopConfig)
        {
            Sequence sequence = new Sequence();

            sequence.AddPreAction(SetupLevelLoop(loopConfig));
            sequence.SetAction(StartLoopWithConfig());

            StartCoroutine(LevelCoroutine(sequence));
        }

        private IEnumerator SetupLevelLoop(LevelLoopSO loopConfig)
        {
            _levelConfig = loopConfig;

            obstacleSequence.SetLevelConfig(_levelConfig);
            obstacleSequence.SetPostAction(StartMinionPhase());

            obstaclesSpawner.gameObject.SetActive(false);
            enemy.SetActive(false);
            minionManager.gameObject.SetActive(false);
            fallingBlockSpawner.SetFallingAttackData(_levelConfig.bossData.fallingAttackData);

            roadManager.HandleNewVelocity(_levelConfig.roadData.roadVelocity);

            yield return null;
        }
        
        public IEnumerator StartLoopWithConfig()
        {
            yield return obstacleSequence.Execute();
        }
    }
}
