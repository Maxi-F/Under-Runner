using System;
using _Dev.GolfTest.Scripts.Events;
using TMPro;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.UI
{
    public class PlayerHealthUIHandler : MonoBehaviour
    {
        [SerializeField] private IntEventChannelSO onPlayerTakeDamageEvent;

        private TextMeshProUGUI _textMesh;
        
        // Start is called before the first frame update
        void Start()
        {
            _textMesh ??= GetComponent<TextMeshProUGUI>();
            onPlayerTakeDamageEvent?.onIntEvent.AddListener(HandleChangeHealth);
        }

        private void OnDisable()
        {
            onPlayerTakeDamageEvent?.onIntEvent.RemoveListener(HandleChangeHealth);
        }

        private void HandleChangeHealth(int value)
        {
            _textMesh.text = $"Player Health: {value} / 100";
        }
    }
}
