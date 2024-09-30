using UnityEngine;

namespace Player.Controllers
{
    [RequireComponent(typeof(PlayerAgent))]
    public class PlayerController : MonoBehaviour
    {
        protected PlayerAgent playerAgent;
        protected Collider playerCollider;

        protected virtual void OnEnable()
        {
            playerAgent ??= GetComponent<PlayerAgent>();
            playerCollider ??= GetComponent<Collider>();
        }
    }
}