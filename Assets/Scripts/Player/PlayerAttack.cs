using System.Collections;
using Events;
using Input;
using Managers;
using Player.Weapon;
using Unity.Mathematics;
using UnityEngine;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private PauseSO pauseData;
        [SerializeField] private InputHandlerSO inputHandler;

        [Header("Animation Handler")]
        [SerializeField] private PlayerAnimationHandler animationHandler;

        [Header("Initial Sequence Events")]
        [SerializeField] private VoidEventChannelSO onCinematicStarted;
        [SerializeField] private VoidEventChannelSO onCinematicFinished;

        [Header("Attack Configuration")]
        [SerializeField] private MeleeWeapon meleeWeapon;
        [SerializeField] private AnimationCurve attackCurve;
        [SerializeField] private LayerMask layers;
        [SerializeField] private float attackDuration;
        [SerializeField] private float attackCoolDown;


        private bool _canAttack = true;
        private Coroutine _attackCoroutine = null;

        private void OnEnable()
        {
            inputHandler.onPlayerAttack.AddListener(HandleAttack);

            onCinematicStarted.onEvent.AddListener(DisableAttack);
            onCinematicFinished.onEvent.AddListener(EnableAttack);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerAttack.RemoveListener(HandleAttack);

            onCinematicStarted.onEvent.RemoveListener(DisableAttack);
            onCinematicFinished.onEvent.RemoveListener(EnableAttack);
        }

        public void HandleAttack()
        {
            if (!_canAttack || pauseData.isPaused)
                return;

            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            _attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            _canAttack = false;

            meleeWeapon.enabled = true;
            float timer = 0;
            float startTime = Time.time;
            animationHandler.StartAttackAnimation();

            while (timer < attackDuration)
            {
                timer = Time.time - startTime;
                animationHandler.SetAttackProgress(attackCurve.Evaluate(timer / attackDuration));
                if (meleeWeapon.enabled && timer / attackDuration > 0.5f)
                    meleeWeapon.enabled = false;

                yield return null;
            }

            meleeWeapon.enabled = false;
            yield return CoolDownCoroutine();
            _canAttack = true;
        }

        private IEnumerator CoolDownCoroutine()
        {
            yield return new WaitForSeconds(attackCoolDown);
        }

        private void EnableAttack()
        {
            _canAttack = true;
        }

        private void DisableAttack()
        {
            _canAttack = false;
        }
    }
}