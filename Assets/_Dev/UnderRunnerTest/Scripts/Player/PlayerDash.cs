using System.Collections;
using _Dev.UnderRunnerTest.Scripts.Input;
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


        private PlayerMovement _movement;
        private CharacterController _characterController;

        private void Awake()
        {
            _movement = GetComponent<PlayerMovement>();
            _characterController = GetComponent<CharacterController>();
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
            //Dash(_movement.CurrentDir);
            StartCoroutine(DashCoroutine());
        }

        private void Dash(Vector3 dir)
        {
            _characterController.Move(dir * dashSpeed);
        }

        private IEnumerator DashCoroutine()
        {
            float startTime = Time.time;
            float timer = 0;
            
            while (timer < dashDuration)
            {
                _characterController.Move(_movement.CurrentDir * (dashSpeed * Time.deltaTime));
                timer = Time.time - startTime;
                yield return null;
            }
        }
    }
}