using System;
using System.Collections.Generic;
using _Dev.UnderRunnerTest.Scripts.Attacks.ParryProjectile;
using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Dev.UnderRunnerTest.Scripts.Enemy.Attacks
{
    public class ParryProjectileAttack : MonoBehaviour, IEnemyAttack
    {
        [SerializeField] private GameObject parryProjectile;
        [SerializeField] private GameObject player;
        [SerializeField] private Vector3 offset;
        [SerializeField] private int attacksBeforeParryCanExecute = 3;
        
        [Header("Parry Projectile directions")] 
        [SerializeField] private List<ParryProjectileFirstForce> firstForces;

        [Header("Events")] [SerializeField] private VoidEventChannelSO onAnotherAttackExecuted;
        
        private Scripts.Attacks.ParryProjectile.ParryProjectile _parryProjectile;
        private int _executedAttacksQuantity;

        private void OnEnable()
        {
            onAnotherAttackExecuted?.onEvent.AddListener(AddExecutedAttack);
        }

        private void OnDisable()
        {
            onAnotherAttackExecuted?.onEvent.RemoveListener(AddExecutedAttack);
        }

        private void AddExecutedAttack()
        {
            _executedAttacksQuantity++;
        }

        public bool CanExecute()
        {
            return _executedAttacksQuantity >= attacksBeforeParryCanExecute;
        }

        public void Execute()
        {
            _executedAttacksQuantity = 0;
            
            GameObject parryProjectileInstance = Instantiate(parryProjectile);
            parryProjectileInstance.transform.position = transform.position + offset;
            
            _parryProjectile = parryProjectileInstance.GetComponent<Scripts.Attacks.ParryProjectile.ParryProjectile>();
            
            _parryProjectile.SetFirstForce(firstForces[Random.Range(0, firstForces.Count)]);
            _parryProjectile.SetObjectToFollow(player);
            _parryProjectile.SetFirstObjectToFollow(player);
        }

        public bool IsExecuting()
        {
            return _parryProjectile.gameObject.activeInHierarchy;
        }
    }
}
