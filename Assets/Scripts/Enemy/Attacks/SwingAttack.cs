using System;
using System.Collections;
using Attacks.Swing;
using UnityEngine;
using Utils;

namespace Enemy.Attacks
{
    public class SwingAttack : MonoBehaviour, IEnemyAttack
    {
        [SerializeField] private Swing swing;
        private bool _canStartAttack;
        
        public void OnEnable()
        {
        }

        public bool CanExecute()
        {
            return true;
        }

        public IEnumerator Execute()
        {
            yield return CreateLaserSequence().Execute();
        }

        private Sequence CreateLaserSequence()
        {
            Sequence laserSequence = new Sequence();
            
            laserSequence.SetAction(StartAttack());

            return laserSequence;
        }
        
        private IEnumerator StartAttack()
        {
            swing.gameObject.SetActive(true);

            yield return new WaitUntil(() => swing.gameObject.activeInHierarchy);
        }
    }
}
