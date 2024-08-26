using _Dev.UnderRunnerTest.Scripts.FSM;
using UnityEngine;

namespace _Dev.UnderRunnerTest.Scripts.Minion.States
{
    public abstract class MinionStateSO : StateSO
    {
        [HideInInspector] public Transform agentTransform;
        [HideInInspector] public GameObject target;
    }
}