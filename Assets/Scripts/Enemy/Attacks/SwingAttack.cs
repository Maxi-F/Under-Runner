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
        [SerializeField] private MeshRenderer bodyMeshRenderer;
        [SerializeField] private Material laserAttackMaterial;
        
        private bool _canStartAttack;
        private Material _startBodyMaterial;
        
        public void OnEnable()
        {
            _startBodyMaterial = bodyMeshRenderer.material;
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
            
            laserSequence.AddPreAction(ChangeBodyMaterial(laserAttackMaterial));
            laserSequence.SetAction(StartAttack());
            laserSequence.AddPostAction(ChangeBodyMaterial(_startBodyMaterial));

            return laserSequence;
        }

        private IEnumerator ChangeBodyMaterial(Material newMaterial)
        {
            bodyMeshRenderer.material = newMaterial;
            yield return null;
        }
        
        private IEnumerator StartAttack()
        {
            yield return swing.SwingSequence();
        }
    }
}
