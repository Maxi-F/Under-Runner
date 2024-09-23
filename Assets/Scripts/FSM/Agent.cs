using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public abstract class Agent : MonoBehaviour
    {
        protected FSM Fsm;
        private Coroutine _updateCoroutine;
        
        protected abstract List<State> GetStates();

        protected virtual void OnEnable()
        { 
            Fsm = new FSM(GetStates());
        }

        protected virtual void Update()
        {
            Fsm.Update();
        }
    }
}
