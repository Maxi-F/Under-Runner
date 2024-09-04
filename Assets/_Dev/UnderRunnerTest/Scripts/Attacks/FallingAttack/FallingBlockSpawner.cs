using System;
using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.LevelManagement;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Dev.UnderRunnerTest.Scripts.Attacks.FallingAttack
{
    public class FallingBlockSpawner : MonoBehaviour
    {
        [Header("Prefab")]
        [SerializeField] private GameObject fallingBlock;

        [Header("Events")] 
        [SerializeField] private Vector3EventChannelSO onPlayerPositionChanged;
        [SerializeField] private VoidEventChannelSO onHandleAttack;
        
        private bool _isSpawning = false;
        private Vector3 _playerPosition;
        private FallingAttackData _fallingAttackData;
        
        private void OnEnable()
        {
            onPlayerPositionChanged?.onVectorEvent.AddListener(HandleNewPlayerPosition);
            onHandleAttack?.onEvent.AddListener(HandleSpawnBlocks);
        }
        
        private void OnDisable()
        {
            onPlayerPositionChanged?.onVectorEvent.RemoveListener(HandleNewPlayerPosition);
            onHandleAttack?.onEvent.RemoveListener(HandleSpawnBlocks);
        }

        public void SetFallingAttackData(FallingAttackData fallingAttackData)
        {
            _fallingAttackData = fallingAttackData;
        }
        
        public bool IsSpawning()
        {
            return _isSpawning;
        }

        private void HandleSpawnBlocks()
        {
            StartCoroutine(SpawnBlocks(_fallingAttackData.spawnQuantity));
        }

        private void HandleNewPlayerPosition(Vector3 playerPosition)
        {
            _playerPosition = playerPosition;
        }

        private IEnumerator SpawnBlocks(int quantity)
        {
            if (_isSpawning) yield break;
            
            _isSpawning = true;

            for (int i = 0; i < quantity; i++)
            {
                GameObject fallingBlockInstance = Instantiate(fallingBlock, transform);
                Vector2 distance = CalculateRandomDistance();
                
                Vector3 fallingBlockPosition = new Vector3(
                    distance.x,
                    fallingBlockInstance.transform.position.y,
                    distance.y
                );

                fallingBlockInstance.transform.position = fallingBlockPosition;

                yield return new WaitForSeconds(_fallingAttackData.timeBetweenSpawns);
            }

            yield return new WaitForSeconds(_fallingAttackData.spawnCooldown);

            _isSpawning = false;
        }

        private Vector2 CalculateRandomDistance()
        {
            float xDistance = Random.Range(_playerPosition.x - _fallingAttackData.spawnRadiusFromPlayer, 
                _playerPosition.x + _fallingAttackData.spawnRadiusFromPlayer);
            float zDistance = Random.Range(_playerPosition.z - _fallingAttackData.spawnRadiusFromPlayer,
                _playerPosition.z + _fallingAttackData.spawnRadiusFromPlayer);

            return new Vector2(xDistance, zDistance);
        }
    }
}
