using System;
using System.Collections;
using Events;
using UnityEngine;

namespace Enemy.Attacks
{
    public class FallingBlockAttack : MonoBehaviour, IEnemyAttack
    {
        [SerializeField] private VoidEventChannelSO onHandleAttack;
        [SerializeField] private VoidEventChannelSO onFinishSpawningBlocks;
        
        private bool _isExecuting;

        private void OnEnable()
        {
            onFinishSpawningBlocks?.onEvent.AddListener(HandleFinishSpawningBlocks);
        }

        private void OnDisable()
        {
            onFinishSpawningBlocks?.onEvent.RemoveListener(HandleFinishSpawningBlocks);
        }

        public bool CanExecute()
        {
            return true;
        }

        public IEnumerator Execute()
        {
            _isExecuting = true;

            onHandleAttack?.RaiseEvent();
            yield return new WaitWhile(() => _isExecuting);
        }

        private void HandleFinishSpawningBlocks()
        {
            Debug.Log("FinishSpawningBlocks");
            _isExecuting = false;
        }
    }
}
