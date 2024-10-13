using System;
using System.Collections.Generic;
using Events;
using Events.ScriptableObjects;
using Health;
using UnityEngine;

namespace LevelManagement
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<LevelLoopSO> loopConfigs;
        [SerializeField] private LevelLoopManager levelLoopManager;
        [SerializeField] private HealthPoints playerHealthPoints;
        [SerializeField] private HealthPoints bossHealthPoints;
        [SerializeField] private String creditsScene = "Credits";
        
        [Header("Events")]
        [SerializeField] private IntEventChannelSO onEnemyDamageEvent;
        [SerializeField] private VoidEventChannelSO onPlayerDeathEvent;
        [SerializeField] private BoolEventChannelSO onTryAgainCanvasEvent;
        [SerializeField] private VoidEventChannelSO onResetGameplayEvent;
        [SerializeField] private StringEventChannelSo onOpenSceneEvent;
        
        private int _loopConfigIndex;
        private LevelLoopSO _actualLoopConfig;
        
        private void Start()
        {
            HandleResetGameplay();
        }

        private void OnEnable()
        {
            onResetGameplayEvent?.onEvent.AddListener(HandleResetGameplay);
            onEnemyDamageEvent?.onIntEvent.AddListener(HandleNextPhase);
            onPlayerDeathEvent?.onEvent.AddListener(HandlePlayerDeath);
        }

        private void OnDisable()
        {
            onResetGameplayEvent?.onEvent.RemoveListener(HandleResetGameplay);
            onEnemyDamageEvent?.onIntEvent.RemoveListener(HandleNextPhase);
            onPlayerDeathEvent?.onEvent.RemoveListener(HandlePlayerDeath);
        }

        private void HandlePlayerDeath()
        {
            onTryAgainCanvasEvent?.RaiseEvent(true);
            levelLoopManager.StopSequence();
        }
        
        private void HandleNextPhase(int hitPointsLeft)
        {
            if (hitPointsLeft < _actualLoopConfig.bossData.hitPointsToNextPhase)
            {
                _loopConfigIndex++;
                SetActualLoop();
                levelLoopManager.StartLevelSequence(_actualLoopConfig);
            }
        }

        private void SetActualLoop()
        {
            if (_loopConfigIndex >= loopConfigs.Count)
            {
                Debug.LogWarning("Loop index more than count. Finishing gameplay");
                HandleFinish();
                return;
            }
            
            _actualLoopConfig = loopConfigs[_loopConfigIndex];
        }

        private void HandleFinish()
        {
            levelLoopManager.StopSequence();
            onOpenSceneEvent.RaiseEvent(creditsScene);
        }

        private void HandleResetGameplay()
        {
            _loopConfigIndex = 0;
            playerHealthPoints.ResetHitPoints();
            bossHealthPoints.ResetHitPoints();
            
            SetActualLoop();
            levelLoopManager.StartLevelSequence(_actualLoopConfig);
        }
    }
}
