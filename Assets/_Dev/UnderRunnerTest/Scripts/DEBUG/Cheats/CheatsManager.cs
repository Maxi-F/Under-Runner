using System;
using _Dev.UnderRunnerTest.Scripts.DEBUG.Input;
using _Dev.UnderRunnerTest.Scripts.Health;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.DEBUG.Cheats
{
    [RequireComponent(typeof(DebugInputReader))]
    public class CheatsManager : MonoBehaviour
    {
#if UNITY_EDITOR || DEVELOPMENT_BUILD || ENABLE_CHEATS

        [SerializeField] private GameObject uiPrefab;
        [SerializeField] private GameObject canvas;

        [SerializeField] private CheatsConfigSO config;
        [SerializeField] private HealthPoints playerHealth;

        private DebugInputReader _inputReader;
        private GameObject _cheatsUI;

        void Start()
        {
            _inputReader = GetComponent<DebugInputReader>();
            _inputReader.OnOpenCheatsMenu += HandleOpenMenu;

            _cheatsUI = Instantiate(uiPrefab, canvas.transform);
            _cheatsUI.SetActive(false);
            config.playerHealth = playerHealth;
        }

        private void OnDestroy()
        {
            _inputReader.OnOpenCheatsMenu += HandleOpenMenu;
        }

        private void HandleOpenMenu()
        {
            _cheatsUI.SetActive(!_cheatsUI.activeInHierarchy);
        }
#endif
    }
}