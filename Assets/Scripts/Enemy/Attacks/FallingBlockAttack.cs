using System.Collections;
using Events;
using UnityEngine;

namespace Enemy.Attacks
{
    public class FallingBlockAttack : MonoBehaviour, IEnemyAttack
    {
        [SerializeField] private VoidEventChannelSO onHandleAttack;

        public bool CanExecute()
        {
            return true;
        }

        public void Execute()
        {
            onHandleAttack?.RaiseEvent();
        }

        public bool IsExecuting()
        {
            return false;
        }
    }
}
