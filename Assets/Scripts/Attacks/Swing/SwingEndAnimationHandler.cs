using Events;
using UnityEngine;

namespace Attacks.Swing
{
    public class SwingEndAnimationHandler : StateMachineBehaviour
    {
        [SerializeField] private VoidEventChannelSO onSwingEndEvent;
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            onSwingEndEvent?.RaiseEvent();
        }
    }
}
