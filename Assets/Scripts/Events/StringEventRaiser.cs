using UnityEngine;

namespace Events
{
    public class StringEventRaiser : MonoBehaviour
    {
        [SerializeField] private StringEventChannelSo eventToRaise;

        public void RaiseEvent(string value)
        {
            eventToRaise?.RaiseEvent(value);
        }
    }
}
