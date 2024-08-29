using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace _Dev.UnderRunnerTest.Scripts.FSM
{
    public class StateSO : ScriptableObject
    {
        [SerializeField] private List<TransitionSO> transitions;
        public UnityEvent onEnter;
        public UnityEvent onUpdate;
        public UnityEvent onExit;

        public List<TransitionSO> Transitions => transitions;

        public virtual void Enter()
        {
            onEnter?.Invoke();
        }

        public virtual void Update()
        {
            onUpdate?.Invoke();
        }

        public virtual void Exit()
        {
            onExit?.Invoke();
        }

        public bool TryGetTransition(StateSO to, out TransitionSO transitionSo)
        {
            foreach (TransitionSO t in transitions)
            {
                if (t.to == to)
                {
                    transitionSo = t;
                    return true;
                }
            }

            transitionSo = null;
            return false;
        }
    }
}