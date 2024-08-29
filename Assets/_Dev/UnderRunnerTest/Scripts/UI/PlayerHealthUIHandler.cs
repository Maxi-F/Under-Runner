using System;
using _Dev.UnderRunnerTest.Scripts.Events;
using TMPro;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.UI
{
    public class PlayerHealthUIHandler : MonoBehaviour
    {
        [SerializeField] private IntEventChannelSO onPlayerTakeDamageEvent;
        [SerializeField] private String entity = "Player";
        
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
            _textMesh.text = $"{entity} Health: {value} / 100";
        }
    }
}
