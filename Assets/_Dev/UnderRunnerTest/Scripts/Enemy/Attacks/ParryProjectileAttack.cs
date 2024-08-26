using System.Collections.Generic;
using _Dev.UnderRunnerTest.Scripts.Attacks.ParryProjectile;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Enemy.Attacks
{
    public class ParryProjectileAttack : MonoBehaviour, IEnemyAttack
    {
        [SerializeField] private GameObject parryProjectile;
        [SerializeField] private GameObject player;
        [SerializeField] private Vector3 offset;

        [Header("Parry Projectile directions")] 
        [SerializeField] private List<ParryProjectileFirstForce> firstForces;

        private Scripts.Attacks.ParryProjectile.ParryProjectile _parryProjectile;
        
        public void Execute()
        {
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
