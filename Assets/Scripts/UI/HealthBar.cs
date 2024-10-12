using System;
using Events;
using Health;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class HealthBar : MonoBehaviour
    {
        [SerializeField] private Slider slider;
        [SerializeField] private bool shouldStartHided;
    
        [Header("Events")]
        [SerializeField] private IntEventChannelSO onTakeDamage;
        [SerializeField] private IntEventChannelSO onResetDamage;
        [SerializeField] private IntEventChannelSO onInitializeSlider;
        
        private bool _wasTriggered = false;

        private void Awake()
        {
            onInitializeSlider?.onIntEvent.AddListener(HandleInit);
        }

        void Start()
        {
            if (shouldStartHided)
            {
                slider.gameObject.SetActive(false);
                _wasTriggered = false;
            }

            onTakeDamage.onIntEvent.AddListener(HandleTakeDamage);
            onResetDamage?.onIntEvent.AddListener(HandleReset);
        }

        private void OnDestroy()
        {
            onTakeDamage?.onIntEvent.RemoveListener(HandleTakeDamage);
            onInitializeSlider?.onIntEvent.RemoveListener(HandleInit);
            onResetDamage?.onIntEvent.RemoveListener(HandleReset);
        }

        public void HandleReset(int currentHp)
        {
            slider.value = currentHp;
        }
    
        private void HandleInit(int maxValue)
        {
            Debug.Log("initialized");
            slider.maxValue = maxValue;
            slider.value = maxValue;
        }
        
        private void HandleTakeDamage(int currentHealth)
        {
            Debug.Log("hi?");
            if (!_wasTriggered)
            {
                slider.gameObject.SetActive(true);
                _wasTriggered = true;
            }

            Debug.Log($"{slider.value}, {currentHealth}");
            slider.value = currentHealth;
        }
    }
}