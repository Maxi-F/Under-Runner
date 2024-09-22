using System.Collections;
using System.Collections.Generic;
using Events;
using FSM;
using Health;
using Minion.Controllers;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Minion
{
    public class MinionAgent : Agent
    {
        [SerializeField] private float timeBetweenStates;

        [SerializeField] private GameObject player;
        [SerializeField] private GameObject model;
        [SerializeField] private GameObjectEventChannelSO onCollidePlayerEventChannel;

        [SerializeField] private HealthPoints healthPoints;

        
        [Header("Controllers")]
        [SerializeField] private MinionAnimationController minionAnimationController;
        
        [SerializeField] private MinionIdleController minionIdleController;
        [SerializeField] private MinionMoveController minionMoveController;
        [SerializeField] private MinionChargeAttackController minionChargeAttackController;
        [SerializeField] private MinionAttackController minionAttackController;
        
        private State _idleState;
        private State _moveState;
        private State _chargeAttackState;
        private State _attackState;

        protected void Awake()
        {
            List<MinionController> controllers = new List<MinionController>()
            {
                minionIdleController,
                minionMoveController,
                minionChargeAttackController,
                minionAttackController
            };

            foreach (MinionController controller in controllers)
            {
                controller.target = player;
            }
        }

        protected override void Update()
        {
            Vector3 rotation = Quaternion.LookRotation(player.transform.position).eulerAngles;
            rotation.x = 0f;
            rotation.z = 0f;
            
            model.transform.rotation = Quaternion.Euler(rotation);
            base.Update();
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
        
        public void ChangeStateToChargeAttack()
        {
            Fsm.ChangeState(_chargeAttackState);
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
            _moveState.EnterAction += minionAnimationController.Aim;
            _moveState.UpdateAction += minionMoveController.OnUpdate;
            _moveState.ExitAction += minionMoveController.Exit;
                
            _chargeAttackState = new State();
            _chargeAttackState.EnterAction += minionChargeAttackController.Enter;
            _chargeAttackState.EnterAction += minionAnimationController.PrepareAttack;
            _chargeAttackState.UpdateAction += minionChargeAttackController.OnUpdate;
            _chargeAttackState.ExitAction += minionChargeAttackController.Exit;
            
            _attackState = new State();
            _attackState.EnterAction += minionAttackController.Enter;
            _attackState.EnterAction += minionAnimationController.Attack;
            _attackState.UpdateAction += minionAttackController.OnUpdate;
            _attackState.ExitAction += minionAttackController.Exit;

            Transition idleToMoveTransition = new Transition(_idleState, _moveState);
            _idleState.AddTransition(idleToMoveTransition);

            Transition moveToChargeAttackTransition = new Transition(_moveState, _chargeAttackState);
            _moveState.AddTransition(moveToChargeAttackTransition);

            Transition chargeAttackToAttackTransition = new Transition(_chargeAttackState, _attackState);
            _chargeAttackState.AddTransition(chargeAttackToAttackTransition);

            Transition attackToIdleTransition = new Transition(_attackState, _idleState);
            _attackState.AddTransition(attackToIdleTransition);
            
            return new List<State>
                ()
                {
                    _idleState,
                    _moveState,
                    _chargeAttackState,
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