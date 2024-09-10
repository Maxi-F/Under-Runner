using Events;
using UnityEngine;

namespace Attacks.Swing
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
