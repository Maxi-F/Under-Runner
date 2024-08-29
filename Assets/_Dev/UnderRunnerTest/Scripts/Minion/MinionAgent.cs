using System;
using System.Collections;
using System.Collections.Generic;
using _Dev.UnderRunnerTest.Scripts.Events;
using _Dev.UnderRunnerTest.Scripts.FSM;
using _Dev.UnderRunnerTest.Scripts.Health;
using _Dev.UnderRunnerTest.Scripts.Minion.States;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

namespace _Dev.UnderRunnerTest.Scripts.Minion
{
    public class MinionAgent : Agent
    {
        [SerializeField] private float timeBetweenStates;

        [SerializeField] private GameObject player;
        [SerializeField] private GameObjectEventChannelSO onCollidePlayerEventChannel;

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

            GetComponent<HealthPoints>().OnDeathEvent.onEvent.AddListener(Die);
        }

        private void OnEnable()
        {
            foreach (StateSO state in config.states)
            {
                state.onEnter.AddListener(StartChangeCoroutine);
            }

            StartChangeCoroutine();
        }

        private void OnDisable()
        {
            foreach (StateSO state in config.states)
            {
                state.onEnter.RemoveListener(StartChangeCoroutine);
            }

            GetComponent<HealthPoints>().OnDeathEvent.onEvent.RemoveListener(Die);
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
            int randomIndex = Random.Range(0, config.states.Count);
            fsm.ChangeState(config.states[randomIndex]);
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

        private void Die()
        {
            Destroy(this.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                onCollidePlayerEventChannel?.RaiseEvent(other.gameObject);
            }
        }
    }
}