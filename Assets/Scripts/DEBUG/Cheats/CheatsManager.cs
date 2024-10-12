using System;
using DEBUG.Input;
using Events;
using Events.ScriptableObjects;
using Health;
using UnityEngine;

namespace DEBUG.Cheats
{
    [RequireComponent(typeof(DebugInputReader))]
    public class CheatsManager : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD || ENABLE_CHEATS
        
        [SerializeField] private CheatsConfigSO config;
        [SerializeField] private HealthPoints playerHealth;

        [Header("Events")] [SerializeField] private BoolEventChannelSO onHandleCheatsUIEvent;
        
        private DebugInputReader _inputReader;

        private bool _isOverlayActive;
        void Start()
        {
            _inputReader = GetComponent<DebugInputReader>();
            _inputReader.OnOpenCheatsMenu += HandleOpenMenu;

            onHandleCheatsUIEvent?.RaiseEvent(false);
            _isOverlayActive = false;
            config.playerHealth = playerHealth;
        }

        private void OnDestroy()
        {
            _inputReader.OnOpenCheatsMenu += HandleOpenMenu;
        }

        private void HandleOpenMenu()
        {
            onHandleCheatsUIEvent?.RaiseEvent(!_isOverlayActive);
            _isOverlayActive = !_isOverlayActive;
        }
#endif
    }
}