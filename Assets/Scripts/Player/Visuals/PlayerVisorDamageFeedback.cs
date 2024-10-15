using System;
using System.Collections;
using Events;
using Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Player.Visuals
{
    public class PlayerVisorDamageFeedback : MonoBehaviour
    {
        [SerializeField] private IntEventChannelSO onPlayerTakenDamage;
        [SerializeField] private Image panelImage;
        [SerializeField] private float duration;

        private Coroutine _damageCoroutine;

        private void OnEnable()
        {
            onPlayerTakenDamage?.onIntEvent.AddListener(HandleDamageFeedback);
        }

        private void OnDisable()
        {
            onPlayerTakenDamage?.onIntEvent.RemoveListener(HandleDamageFeedback);
        }

        private void HandleDamageFeedback(int damageReceived)
        {
            if (_damageCoroutine != null)
                StopCoroutine(_damageCoroutine);

            _damageCoroutine = StartCoroutine(DamageFeedback());
        }

        private IEnumerator DamageFeedback()
        {
            panelImage.enabled = true;
            yield return new WaitForSeconds(duration);
            panelImage.enabled = false;
        }
    }
}