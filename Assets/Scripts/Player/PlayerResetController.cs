using Events;
using Health;
using UnityEngine;

namespace Player
{
    public class PlayerResetController : MonoBehaviour
    {
        [SerializeField] private Vector3 initPosition;
        [SerializeField] private HealthPoints healthPoints;
        [SerializeField] private PlayerDashController dashController;
        
        [Header("Events")] 
        [SerializeField] private VoidEventChannelSO onPlayerDeathEvent;
        [SerializeField] private VoidEventChannelSO onGameplayResetEvent;
        
        private void Awake()
        {
            onGameplayResetEvent?.onEvent.AddListener(HandleResetPlayer);
        }
        
        private void OnEnable()
        {
            transform.position = initPosition;
            onPlayerDeathEvent?.onEvent.AddListener(DisablePlayer);
        }

        protected void OnDisable()
        {
            onPlayerDeathEvent?.onEvent.RemoveListener(DisablePlayer);
        }

        private void OnDestroy()
        {
            onGameplayResetEvent?.onEvent.RemoveListener(HandleResetPlayer);
        }

        private void HandleResetPlayer()
        {
            healthPoints.ResetHitPoints();
            dashController.ResetDash();
            gameObject.SetActive(true);
        }

        private void DisablePlayer()
        {
            gameObject.SetActive(false);
        }
    }
}
