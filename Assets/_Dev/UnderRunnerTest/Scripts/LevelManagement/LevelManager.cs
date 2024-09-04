using System;
using System.Collections.Generic;
using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.LevelManagement
{
    public class LevelManager : MonoBehaviour
    {
        [SerializeField] private List<LevelLoopSO> loopConfigs;
        [SerializeField] private LevelLoopManager levelLoopManager;
        
        [Header("Events")]
        [SerializeField] private IntEventChannelSO onEnemyDamageEvent;
        
        private int _loopConfigIndex;
        private LevelLoopSO _actualLoopConfig;
        
        private void Start()
        {
            _loopConfigIndex = 0;
            SetActualLoop();
            levelLoopManager.StartLoopWithConfig(_actualLoopConfig);
        }

        private void OnEnable()
        {
            onEnemyDamageEvent?.onIntEvent.AddListener(HandleNextPhase);
        }

        private void OnDisable()
        {
            onEnemyDamageEvent?.onIntEvent.RemoveListener(HandleNextPhase);
        }

        private void HandleNextPhase(int hitPointsLeft)
        {
            Debug.Log("On Handle Next Phase");
            if (hitPointsLeft < _actualLoopConfig.bossData.hitPointsToNextPhase)
            {
                Debug.Log("NEXT PHASE!!!!");
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
