using Health;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] protected EnemyAgent enemyAgent;

        [Header("Animator Handler")]
        [SerializeField] protected EnemyAnimationHandler animationHandler;
    }
}