using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Attacks.Swing
{
    public class Swing : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private VoidEventChannelSO onSwingEndEvent;

        private void OnEnable()
        {
            onSwingEndEvent?.onEvent.AddListener(HandleSwingEndEvent);
        }

        private void OnDisable()
        {
            onSwingEndEvent?.onEvent.RemoveListener(HandleSwingEndEvent);
        }

        private void HandleSwingEndEvent()
        {
            gameObject.SetActive(false);
        }
    }
}
