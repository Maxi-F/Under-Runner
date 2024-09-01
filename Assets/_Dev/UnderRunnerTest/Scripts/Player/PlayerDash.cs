using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Health;
using _Dev.UnderRunnerTest.Scripts.Input;
using UnityEditor.Rendering;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Dev.UnderRunnerTest.Scripts.Player
{
    public class PlayerDash : MonoBehaviour
    {
        [SerializeField] private InputHandlerSO inputHandler;

        [FormerlySerializedAs("dashLength")] [Header("Dash Configuration")] [SerializeField]
        private float dashSpeed;

        [SerializeField] private float dashDuration;
        [SerializeField] private float dashCoolDown;
        [SerializeField] private AnimationCurve speedCurve;

        private PlayerMovement _movement;
        private CharacterController _characterController;
        private HealthPoints _healthPoints;

        private bool _canDash = true;
        private Coroutine _dashCoroutine = null;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _characterController = GetComponent<CharacterController>();

            _healthPoints ??= GetComponent<HealthPoints>();
        }

        private void OnEnable()
        {
            inputHandler.onPlayerDash.AddListener(HandleDash);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerDash.RemoveListener(HandleDash);
        }

        private void HandleDash()
        {
            if (!_canDash)
                return;

            if (_dashCoroutine != null)
                StopCoroutine(_dashCoroutine);

            _healthPoints.SetIsInvincible(true);
            _dashCoroutine = StartCoroutine(DashCoroutine());
        }

        private void Dash(Vector3 dir)
        {
        }

        private IEnumerator DashCoroutine()
        {
            float startTime = Time.time;
            float timer = 0;
            _canDash = false;

            Vector3 dashDir = _movement.CurrentDir;
            _movement.ToggleMoveability(false);
            while (timer < dashDuration)
            {
                float dashTime = Mathf.Lerp(0, 1, timer / dashDuration);
                _characterController.Move(dashDir * (dashSpeed * speedCurve.Evaluate(dashTime) * Time.deltaTime));
               // _characterController.Move(dashDir * (dashSpeed * Time.deltaTime));
                timer = Time.time - startTime;
                yield return null;
            }

            _movement.ToggleMoveability(true);
            _healthPoints.SetIsInvincible(false);
            yield return CoolDownCoroutine();
            _canDash = true;
        }

        private IEnumerator CoolDownCoroutine()
        {
            yield return new WaitForSeconds(dashCoolDown);
        }
    }
}