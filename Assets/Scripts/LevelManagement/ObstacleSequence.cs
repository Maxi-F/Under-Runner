using System.Collections;
using Events;
using ObstacleSystem;
using Roads;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace LevelManagement
{
    public class ObstacleSequence : MonoBehaviour
    {
        [SerializeField] private RoadManager roadManager;
        
        [Header("Spawners")]
        [SerializeField] private ObstaclesSpawner obstaclesSpawner;

        [Header("UI")]
        [SerializeField] private Slider progressBar;

        private bool _isObstacleSystemDisabled;
        private LevelLoopSO _levelConfig;
        private IEnumerator _postAction;
        
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onObstaclesSystemDisabled;
        
        private void OnEnable()
        {
            onObstaclesSystemDisabled.onEvent.AddListener(HandleObstacleSystemDisabled);
        }

        private void OnDisable()
        {
            if (obstaclesSpawner != null)
                onObstaclesSystemDisabled.onEvent.RemoveListener(HandleObstacleSystemDisabled);
        }

        public void SetupSequence(RoadData roadData)
        {
            obstaclesSpawner.gameObject.SetActive(false);
            roadManager.HandleNewVelocity(roadData.roadVelocity);
        }
        
        private void HandleObstacleSystemDisabled()
        {
            _isObstacleSystemDisabled = true;
        }
        
        public IEnumerator Execute()
        {
            return GetObstacleSequence().Execute();
        }

        public void SetLevelConfig(LevelLoopSO levelConfig)
        {
            _levelConfig = levelConfig;
        }

        public void SetPostAction(IEnumerator postAction)
        {
            _postAction = postAction;
            Debug.Log(_postAction);
        }

        private Sequence GetObstacleSequence()
        {
            Sequence obstacleSequence = new Sequence();

            obstacleSequence.AddPreAction(ObstacleSequencePreActions());
            obstacleSequence.SetAction(ObstaclesAction());
            obstacleSequence.AddPostAction(ObstaclesPostActions());
            obstacleSequence.AddPostAction(_postAction);
            
            return obstacleSequence;
        }

    
        private IEnumerator ObstacleSequencePreActions()
        {
            obstaclesSpawner.gameObject.SetActive(true);
            progressBar.gameObject.SetActive(true);
            _isObstacleSystemDisabled = false;

            yield return null;
        }

        private IEnumerator ObstaclesAction()
        {
            float timer = 0;
            float obstaclesDuration = _levelConfig.obstacleData.obstaclesDuration;
            float obstacleCooldown = _levelConfig.obstacleData.obstacleCooldown;
            float startTime = Time.time;

            obstaclesSpawner.StartWithCooldown(obstacleCooldown, _levelConfig.obstacleData.minDistance);

            while (timer < obstaclesDuration)
            {
                timer = Time.time - startTime;
                progressBar.value = Mathf.Lerp(0, progressBar.maxValue, timer / obstaclesDuration);
                yield return null;
            }

            progressBar.gameObject.SetActive(false);
            obstaclesSpawner.Disable();
        }

        private IEnumerator ObstaclesPostActions()
        {
            yield return new WaitUntil(() => _isObstacleSystemDisabled);
        }
    }
}
