using System;
using System.Collections.Generic;
using UnityEngine;

namespace FSM
{
    public abstract class Agent : MonoBehaviour
    {
        [SerializeField] protected AgentConfigSO config;

        protected FSM fsm;

        protected virtual void Awake()
        {
            fsm = new FSM(config.states);
        }

        protected virtual void Update()
        {
            fsm.Update();
        }
    }
}