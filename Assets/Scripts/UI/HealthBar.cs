using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using Health;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthPoints health;
    [SerializeField] private Slider slider;
    [SerializeField] private IntEventChannelSO onTakeDamage;
    [SerializeField] private IntEventChannelSO onResetDamage;
    [SerializeField] private bool shouldStartHided;
    private bool _wasTriggered = false;

    void Start()
    {
        if (shouldStartHided)
        {
            slider.gameObject.SetActive(false);
            _wasTriggered = false;
        }

        slider.maxValue = health.MaxHealth;
        slider.value = health.MaxHealth;

        onTakeDamage.onIntEvent.AddListener(HandleTakeDamage);
        onResetDamage?.onIntEvent.AddListener(HandleReset);
    }

    private void OnDestroy()
    {
        onTakeDamage?.onIntEvent.RemoveListener(HandleTakeDamage);
        onResetDamage?.onIntEvent.RemoveListener(HandleReset);
    }

    private void HandleReset(int currentHp)
    {
        slider.value = currentHp;
    }
    
    private void HandleTakeDamage(int damage)
    {
        if (!_wasTriggered)
        {
            slider.gameObject.SetActive(true);
            _wasTriggered = true;
        }

        slider.value = health.CurrentHp;
    }
}