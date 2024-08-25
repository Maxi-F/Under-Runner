using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Attacks.Swing;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Enemy.Attacks
{
    public class SwingAttack : MonoBehaviour, IEnemyAttack
    {
        [SerializeField] private Swing swing;

        private bool _isExecuting;
    
        public void Execute()
        {
            _isExecuting = true;

            StartCoroutine(StartAttack());
        }

        public bool IsExecuting()
        {
            return _isExecuting;
        }
    
        private IEnumerator StartAttack()
        {
            // TODO fix this encapsulation problem
        
            swing.gameObject.SetActive(true);

            yield return new WaitUntil(() => swing.gameObject.activeInHierarchy);

            _isExecuting = false;
        }
    }
}
