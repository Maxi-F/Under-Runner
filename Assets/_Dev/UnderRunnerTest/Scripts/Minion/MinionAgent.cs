using System;
using System.Collections;
using System.Collections.Generic;
using _Dev.UnderRunnerTest.Scripts.FSM;
using _Dev.UnderRunnerTest.Scripts.Minion.States;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Random = UnityEngine.Random;

namespace _Dev.UnderRunnerTest.Scripts.Minion
{
    public class MinionAgent : Agent
    {
        [SerializeField] private float timeBetweenStates;

        [SerializeField] private GameObject player;

        protected override void Awake()
        {
            base.Awake();
            StartChangeCoroutine();

            foreach (StateSO state in config.states)
            {
                (state as MinionStateSO).target = player;
                (state as MinionStateSO).agentTransform = transform;
                (state as MinionStateSO).onCoroutineCall += HandleCallCoroutine;
            }
        }

        private void OnEnable()
        {
            foreach (StateSO state in config.states)
            {
                state.onEnter.AddListener(StartChangeCoroutine);
            }
        }

        private void OnDisable()
        {
            foreach (StateSO state in config.states)
            {
                state.onEnter.RemoveListener(StartChangeCoroutine);
            }
        }

        private void HandleCallCoroutine(IEnumerator coroutine)
        {
            StartCoroutine(coroutine);
        }

        private void StartChangeCoroutine()
        {
            StartCoroutine(ChangeStateCoroutine());
        }

        private IEnumerator ChangeStateCoroutine()
        {
            yield return new WaitForSeconds(timeBetweenStates);
            if (Random.Range(0, 2) == 0)
                ChangeToMoveState();
            else
                ChangeToIdleState();
        }

        [ContextMenu("Move")]
        private void ChangeToMoveState()
        {
            fsm.ChangeState(fsm.FindState<MinionMoveStateSO>());
        }

        [ContextMenu("Idle")]
        private void ChangeToIdleState()
        {
            fsm.ChangeState(fsm.FindState<MinionIdleStateSO>());
        }
    }
}