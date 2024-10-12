using System;
using Events;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Slider))]
    public class ProgressBar : MonoBehaviour
    {
        [Header("Events")] 
        [SerializeField] private BoolEventChannelSO onProgressBarActiveEvent;
        [SerializeField] private FloatEventChannelSO onProgressBarValueEvent;
        
        private Slider _slider;
        private void Awake()
        {
            _slider ??= GetComponent<Slider>();
        }

        private void OnEnable()
        {
            onProgressBarActiveEvent?.onBoolEvent.AddListener(HandleProgressBarActive);
            onProgressBarValueEvent?.onFloatEvent.AddListener(HandleProgressBarValueEvent);
        }

        private void OnDisable()
        {
            onProgressBarActiveEvent?.onBoolEvent.AddListener(HandleProgressBarActive);
            onProgressBarValueEvent?.onFloatEvent.AddListener(HandleProgressBarValueEvent);
        }

        private void HandleProgressBarValueEvent(float value)
        {
            _slider.value = value;
        }

        private void HandleProgressBarActive(bool value)
        {
            gameObject.SetActive(value);
        }
    }
}
