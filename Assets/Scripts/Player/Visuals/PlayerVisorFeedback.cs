using System.Collections;
using Events;
using Events.ScriptableObjects;
using UnityEngine;
using UnityEngine.UI;

namespace Player.Visuals
{
    public class PlayerVisorFeedback : MonoBehaviour
    {
        [SerializeField] private VoidEventChannelSO onReactionEvent;
        [SerializeField] private Image panelImage;
        [SerializeField] private float duration;

        private Coroutine _feedbackCoroutine;

        private void OnEnable()
        {
            onReactionEvent?.onEvent.AddListener(HandleFeedback);
        }

        private void OnDisable()
        {
            onReactionEvent?.onEvent.RemoveListener(HandleFeedback);
        }

        private void HandleFeedback()
        {
            if (_feedbackCoroutine != null)
                StopCoroutine(_feedbackCoroutine);

            _feedbackCoroutine = StartCoroutine(Feedback());
        }

        private IEnumerator Feedback()
        {
            panelImage.enabled = true;
            yield return new WaitForSeconds(duration);
            panelImage.enabled = false;
        }
    }
}