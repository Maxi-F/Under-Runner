using System.Collections;
using Attacks.Swing;
using UnityEngine;

namespace Enemy.Attacks
{
    public class SwingAttack : MonoBehaviour, IEnemyAttack
    {
        [SerializeField] private Swing swing;

        private bool _isExecuting;

        public bool CanExecute()
        {
            return true;
        }

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
