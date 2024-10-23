using System;
using Events.ScriptableObjects;
using UnityEngine;

namespace Attacks.ParryProjectile
{
    public class ParryBombAnimation : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private EventChannelSO<bool> onParried;
        private static readonly int PlayerHits = Animator.StringToHash("player_hits");
        private static readonly int BossHits = Animator.StringToHash("boss_hits");

        private void OnEnable()
        {
            onParried?.onTypedEvent.AddListener(HandleParriedAnimation);
        }

        private void OnDisable()
        {
            onParried?.onTypedEvent.RemoveListener(HandleParriedAnimation);
        }

        private void HandleParriedAnimation(bool isTargetingPlayer)
        {
            Debug.Log($"HI? {isTargetingPlayer}");
            animator.ResetTrigger(isTargetingPlayer ? PlayerHits : BossHits);
            animator.SetTrigger(isTargetingPlayer ? BossHits : PlayerHits);
        }
    }
}
