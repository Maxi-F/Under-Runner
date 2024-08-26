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


        protected override void Awake()
        {
            base.Awake();
            StartChangeCoroutine();
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

        private void StartChangeCoroutine()
        {
            Debug.Log("Start Change");
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
            foreach (StateSO state in config.states)
            {
                if (state as MinionMoveStateSO != null)
                {
                    fsm.ChangeState(state);
                }
            }
        }

        [ContextMenu("Idle")]
        private void ChangeToIdleState()
        {
            foreach (StateSO state in config.states)
            {
                if (state as MinionIdleStateSO != null)
                {
                    fsm.ChangeState(state);
                }
            }
        }
    }
}