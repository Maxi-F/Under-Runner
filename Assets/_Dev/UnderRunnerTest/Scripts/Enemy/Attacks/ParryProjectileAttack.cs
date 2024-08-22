using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Enemy.Attacks
{
    public class ParryProjectileAttack : MonoBehaviour, IEnemyAttack
    {
        [SerializeField] private GameObject parryProjectile;
        [SerializeField] private GameObject player;
        [SerializeField] private Vector3 offset;
        
        public void Execute()
        {
            GameObject parryProjectileInstance = Instantiate(parryProjectile);
            parryProjectileInstance.transform.position = transform.position + offset;
            
            ParryProjectile.ParryProjectile parryConfig = parryProjectileInstance.GetComponent<ParryProjectile.ParryProjectile>();
            
            parryConfig.SetObjectToFollow(player);
        }

        public bool IsExecuting()
        {
            return false;
        }
    }
}
