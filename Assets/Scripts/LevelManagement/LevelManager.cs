using System;
using System.Collections.Generic;
using Events;
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
        [SerializeField] private string playerDeathGoToScene = "Menu";
        
        [Header("Events")]
        [SerializeField] private IntEventChannelSO onEnemyDamageEvent;
        [SerializeField] private VoidEventChannelSO onPlayerDeathEvent;
        [SerializeField] private StringEventChannelSo onChangeSceneEvent;
        
        private int _loopConfigIndex;
        private LevelLoopSO _actualLoopConfig;
        
        private void Start()
        {
            _loopConfigIndex = 0;
            playerHealthPoints.ResetHitPoints();
            bossHealthPoints.ResetHitPoints();
            
            SetActualLoop();
            levelLoopManager.StartLoopWithConfig(_actualLoopConfig);
        }

        private void OnEnable()
        {
            onEnemyDamageEvent?.onIntEvent.AddListener(HandleNextPhase);
            onPlayerDeathEvent?.onEvent.AddListener(HandlePlayerDeath);
        }

        private void OnDisable()
        {
            onEnemyDamageEvent?.onIntEvent.RemoveListener(HandleNextPhase);
            onPlayerDeathEvent?.onEvent.RemoveListener(HandlePlayerDeath);
        }

        private void HandlePlayerDeath()
        {
            onChangeSceneEvent?.RaiseEvent(playerDeathGoToScene);
        }
        
        private void HandleNextPhase(int hitPointsLeft)
        {
            if (hitPointsLeft < _actualLoopConfig.bossData.hitPointsToNextPhase)
            {
                _loopConfigIndex++;
                SetActualLoop();
                levelLoopManager.StartLoopWithConfig(_actualLoopConfig);
            }
        }

        private void SetActualLoop()
        {
            if (_loopConfigIndex >= loopConfigs.Count)
            {
                Debug.LogWarning("Loop index more than count");
                return;
            }
            
            _actualLoopConfig = loopConfigs[_loopConfigIndex];
        }
    }
}
