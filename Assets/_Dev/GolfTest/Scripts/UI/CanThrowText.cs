using System;
using _Dev.GolfTest.Scripts.Events;
using TMPro;
using UnityEngine;

namespace _Dev.GolfTest.Scripts.UI
{
    public class CanThrowText : MonoBehaviour
    {
        [SerializeField] private BoolEventChannelSO canThrowChannel;

        private bool _canThrow = false;
        private TextMeshProUGUI _textMesh;
        
        private void OnEnable()
        {
            _textMesh ??= GetComponent<TextMeshProUGUI>();
            
            canThrowChannel.onBoolEvent.AddListener(HandleCanThrow);
        }

        private void OnDisable()
        {
            canThrowChannel.onBoolEvent.RemoveListener(HandleCanThrow);
        }

        private void HandleCanThrow(bool value)
        {
            _canThrow = value;

            _textMesh.text = $"Can Throw: {_canThrow.ToString()}";
        }
    }
}
