using System;
using System.Collections;
using FSM;
using UnityEngine;

namespace Minion.States
{
    public abstract class MinionStateSO : StateSO
    {
        [HideInInspector] public Transform agentTransform;
        [HideInInspector] public GameObject target;

        public Action<IEnumerator> onCoroutineCall;

        protected void CallCoroutine(IEnumerator coroutine)
        {
            onCoroutineCall?.Invoke(coroutine);
        }
    }
}