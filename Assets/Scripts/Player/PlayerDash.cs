using System;
using System.Collections;
using Health;
using Input;
#if UNITY_EDITOR
using UnityEditor.Rendering;
using UnityEditor.UIElements;
#endif
using UnityEngine;
#if UNITY_EDITOR
using UnityEngine.Serialization;
#endif

namespace Player
{
    public class PlayerDash : MonoBehaviour
    {
        [SerializeField] private InputHandlerSO inputHandler;

        [Header("Dash Configuration")]
        [SerializeField] private float dashSpeed;

        [SerializeField] private float dashDuration;
        [SerializeField] private float dashCoolDown;
        [SerializeField] private AnimationCurve speedCurve;

        [Header("Bullet Time Dash")]
        [SerializeField] private DashPredictionLine dashPredictionLine;

        [SerializeField] private AnimationCurve bulletTimeVariationCurve;
        [SerializeField] private float bulletTimeDuration;

        private PlayerMovement _movement;
        private HealthPoints _healthPoints;

        private bool _canDash = true;
        private Coroutine _dashCoroutine = null;
        private Coroutine _bulletTimeCoroutine = null;

        private Rigidbody _rb;
        private bool _isDashing = false;
        private Vector3 _dashDir;
        private float _currentDashSpeed;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _rb = GetComponent<Rigidbody>();

            _healthPoints ??= GetComponent<HealthPoints>();
        }

        private void OnEnable()
        {
            inputHandler.onPlayerDash.AddListener(HandleDash);

            inputHandler.onPlayerDashStarted.AddListener(HandleBulletTimeDashStart);
            inputHandler.onPlayerDashFinished.AddListener(HandleBulletTimeDashFinish);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerDash.RemoveListener(HandleDash);
        }

        private void FixedUpdate()
        {
            if (_isDashing)
                _rb.Move(_rb.position + _dashDir * (_currentDashSpeed * Time.fixedDeltaTime), _rb.rotation);
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

        public void HandleBulletTimeDashStart()
        {
            if (!_canDash)
                return;

            if (_bulletTimeCoroutine != null)
                StopCoroutine(_bulletTimeCoroutine);

            _bulletTimeCoroutine = StartCoroutine(BulletTimeCoroutine());
        }

        public void HandleBulletTimeDashFinish()
        {
            if (!_canDash || Time.timeScale == 1)
                return;

            if (_bulletTimeCoroutine != null)
                StopCoroutine(_bulletTimeCoroutine);
            Time.timeScale = 1f;

            if (_dashCoroutine != null)
                StopCoroutine(_dashCoroutine);

            dashPredictionLine.ToggleVisibility(false);
            _healthPoints.SetIsInvincible(true);
            _dashCoroutine = StartCoroutine(DashCoroutine());
        }

        private IEnumerator DashCoroutine()
        {
            float startTime = Time.time;
            float timer = 0;
            _canDash = false;

            _dashDir = _movement.CurrentDir;
            _movement.ToggleMoveability(false);
            _isDashing = true;
            while (timer < dashDuration)
            {
                float dashTime = Mathf.Lerp(0, 1, timer / dashDuration);

                _currentDashSpeed = dashSpeed * speedCurve.Evaluate(dashTime);
                timer = Time.time - startTime;
                yield return null;
            }

            _isDashing = false;
            _movement.ToggleMoveability(true);
            _healthPoints.SetIsInvincible(false);
            yield return CoolDownCoroutine();
            _canDash = true;
        }

        private IEnumerator CoolDownCoroutine()
        {
            yield return new WaitForSeconds(dashCoolDown);
        }

        private IEnumerator BulletTimeCoroutine()
        {
            float timer = 0;
            float startTime = Time.time;
            dashPredictionLine.ToggleVisibility(true);

            while (timer < bulletTimeDuration)
            {
                timer = Time.time - startTime;
                float timerProgress = Mathf.Lerp(0, 1, timer / bulletTimeDuration);
                Time.timeScale = bulletTimeVariationCurve.Evaluate(timerProgress);
                yield return null;
            }

            dashPredictionLine.ToggleVisibility(false);
            Time.timeScale = 1;
        }

        private void OnCollisionEnter(Collision other)
        {
            if (!other.gameObject.CompareTag("Wall"))
                return;

            _isDashing = false;

            // if (other.transform.position.x < 0 || other.transform.position.x > 0)
            // {
            //     float newX = transform.position.x - (other.transform.position.x - transform.position.x);
            //     transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            // }
            // else if (other.transform.position.z > 0 || other.transform.position.z < 0)
            // {
            //     float newZ = other.transform.position.z - transform.position.z;
            //     transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - newZ);
            // }
        }
    }
}