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
        
        public void Execute()
        {
            GameObject parryProjectileInstance = Instantiate(parryProjectile);
            parryProjectileInstance.transform.position = transform.position + offset;
            
            Scripts.Attacks.ParryProjectile.ParryProjectile parryConfig = parryProjectileInstance.GetComponent<Scripts.Attacks.ParryProjectile.ParryProjectile>();
            
            parryConfig.SetFirstForce(firstForces[Random.Range(0, firstForces.Count)]);
            parryConfig.SetObjectToFollow(player);
        }

        public bool IsExecuting()
        {
            return false;
        }
    }
}
