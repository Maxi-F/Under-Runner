using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Enemy.Attacks
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
