using System;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.FSM
{
    [CreateAssetMenu(fileName = "Transition", menuName = "FSM/Transition", order = 0)]
    public class TransitionSO : ScriptableObject
    {
        public StateSO from;
        public StateSO to;
        public event Action transitionAction;

        public void Do()
        {
            from.Exit();
            transitionAction?.Invoke();
            to.Enter();
        }
    }
}