using _Dev.UnderRunnerTest.Scripts.Events;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Attacks.Swing
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
