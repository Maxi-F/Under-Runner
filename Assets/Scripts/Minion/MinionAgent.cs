using System.Collections;
using System.Collections.Generic;
using Events;
using FSM;
using Health;
using Minion.States;
using UnityEngine;
using UnityEngine.Events;

namespace Minion
{
    public class MinionAgent : Agent
    {
        [SerializeField] private float timeBetweenStates;

        [SerializeField] private GameObject player;
        [SerializeField] private GameObjectEventChannelSO onCollidePlayerEventChannel;

        [SerializeField] private HealthPoints healthPoints;
        
        [SerializeField] private MinionIdleController minionIdleController;
        [SerializeField] private MinionMoveController minionMoveController;
        [SerializeField] private MinionAttackController minionAttackController;

        private State _idleState;
        private State _moveState;
        private State _attackState;

        protected void Awake()
        {
            List<MinionController> controllers = new List<MinionController>()
            {
                minionIdleController,
                minionMoveController,
                minionAttackController
            };

            foreach (MinionController controller in controllers)
            {
                controller.target = player;
                controller.agentTransform = transform;
            }
        }
        
        protected override void OnEnable()
        {
            base.OnEnable();
            
            healthPoints?.OnDeathEvent.onEvent.AddListener(Die);
        }

        protected void OnDisable()
        {
            healthPoints?.OnDeathEvent.onEvent.RemoveListener(Die);
            healthPoints?.ResetHitPoints();
        }

        public void ChangeStateToMove()
        {
            Fsm.ChangeState(_moveState);
        }

        public void ChangeStateToAttack()
        {
            Fsm.ChangeState(_attackState);
        }

        public void ChangeStateToIdle()
        {
             Fsm.ChangeState(_idleState);
        }
        
        protected override List<State> GetStates()
        {
            _idleState = new State();
            _idleState.EnterAction += minionIdleController.Enter;
            _idleState.UpdateAction += minionIdleController.OnUpdate;
            _idleState.ExitAction += minionIdleController.Exit;
            
            _moveState = new State();
            _moveState.EnterAction += minionMoveController.Enter;
            _moveState.UpdateAction += minionMoveController.OnUpdate;
            _moveState.ExitAction += minionMoveController.Exit;
                
            _attackState = new State();
            _attackState.EnterAction += minionAttackController.Enter;
            _attackState.UpdateAction += minionAttackController.OnUpdate;
            _attackState.ExitAction += minionAttackController.Exit;

            Transition idleToMoveTransition = new Transition(_idleState, _moveState);
            _idleState.AddTransition(idleToMoveTransition);

            Transition moveToAttackTransition = new Transition(_moveState, _attackState);
            _moveState.AddTransition(moveToAttackTransition);

            Transition attackToIdleTransition = new Transition(_attackState, _idleState);
            _attackState.AddTransition(attackToIdleTransition);
            
            return new List<State>
                ()
                {
                    _idleState,
                    _moveState,
                    _attackState
                };
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