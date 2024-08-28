using System;
using System.Collections;
using System.Collections.Generic;
using _Dev.UnderRunnerTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private HealthPoints health;
    [SerializeField] private Slider slider;
    [SerializeField] private IntEventChannelSO onTakeDamage;

    private bool _wasTriggered = false;

    void Start()
    {
        slider.gameObject.SetActive(false);
        _wasTriggered = false;
        slider.maxValue = health.MaxHealth;

        onTakeDamage.onIntEvent.AddListener(HandleTakeDamage);
    }

    private void OnDestroy()
    {
        onTakeDamage?.onIntEvent.RemoveListener(HandleTakeDamage);
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