using Health;
using UnityEngine;

namespace Enemy
{
    public class EnemyController : MonoBehaviour
    {
        [SerializeField] protected EnemyAgent enemyAgent;
        [SerializeField] protected HealthPoints healthPoints;
    }
}