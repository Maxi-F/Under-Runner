using System;
using System.Collections;
using FSM;
using UnityEngine;

namespace Minion.States
{
    public abstract class MinionController : MonoBehaviour
    {
        [HideInInspector] public Transform agentTransform;
        [HideInInspector] public GameObject target;

        public abstract void Enter();
        public abstract void OnUpdate();
        public abstract void Exit();
    }
}