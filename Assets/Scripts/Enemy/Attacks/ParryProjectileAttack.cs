using System;
using System.Collections;
using System.Collections.Generic;
using Attacks.ParryProjectile;
using Events;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemy.Attacks
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
        
        private ParryBomb _parryBomb;
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

        public IEnumerator Execute()
        {
            _executedAttacksQuantity = 0;
            
            GameObject parryProjectileInstance = Instantiate(parryProjectile);
            parryProjectileInstance.transform.position = transform.position + offset;
            
            _parryBomb = parryProjectileInstance.GetComponent<ParryBomb>();
            
            _parryBomb.SetFirstForce(firstForces[Random.Range(0, firstForces.Count)]);
            _parryBomb.SetObjectToFollow(player);
            _parryBomb.SetFirstObjectToFollow(player);

            yield return new WaitUntil(() => !_parryBomb.gameObject.activeInHierarchy);
        }
    }
}
