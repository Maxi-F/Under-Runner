using System;
using _Dev.GolfTest.Scripts.Events;
using TMPro;
using UnityEngine;

namespace _Dev.GolfTest.Scripts.UI
{
    public class PowerText : MonoBehaviour
    {
        [SerializeField] private FloatEventChannelSO powerChangedChannelSo;

        private float _power;
        private TextMeshProUGUI _textMesh;
        
        private void OnEnable()
        {
            _textMesh ??= GetComponent<TextMeshProUGUI>();
            
            powerChangedChannelSo.onFloatEvent.AddListener(HandlePowerChanged);
        }

        private void OnDisable()
        {
            powerChangedChannelSo.onFloatEvent.RemoveListener(HandlePowerChanged);
        }

        private void HandlePowerChanged(float value)
        {
            _power = value;

            _textMesh.text = $"Power: {_power.ToString("0.00")} / 10";
        }
    }
}