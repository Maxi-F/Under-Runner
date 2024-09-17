using System;
using System.Collections;
using Health;
using Input;
using ParryProjectile;
using Player.Weapon;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerAttack : MonoBehaviour
    {
        [SerializeField] private InputHandlerSO inputHandler;

        [Header("Attack Configuration")]
        [SerializeField] private float attackAmplitude;

        [SerializeField] private GameObject meleeWeaponPivot;
        [SerializeField] private MeleeWeapon meleeWeapon;
        [SerializeField] private float attackRadius;
        [SerializeField] private float attackDuration;
        [SerializeField] private float attackCoolDown;
        [SerializeField] private LayerMask layers;

        private bool _canAttack = true;
        private Coroutine _attackCoroutine = null;

        private void OnEnable()
        {
            inputHandler.onPlayerAttack.AddListener(HandleAttack);
        }

        private void OnDisable()
        {
            inputHandler.onPlayerAttack.RemoveListener(HandleAttack);
        }

        public void HandleAttack()
        {
            if (!_canAttack)
                return;

            if (_attackCoroutine != null)
                StopCoroutine(_attackCoroutine);

            _attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            _canAttack = false;
            float minAngle = 90 - attackAmplitude / 2;
            float maxAngle = -90 - attackAmplitude / 2;
            minAngle = -minAngle;

            quaternion startRotation = Quaternion.Euler(0, -(90 - attackAmplitude / 2), 0);
            quaternion finalRotation = Quaternion.Euler(0, -(90 + attackAmplitude / 2), 0);
            meleeWeaponPivot.transform.localRotation = startRotation;
            meleeWeaponPivot.SetActive(true);

            float timer = 0;
            float startTime = Time.time;

            while (timer < attackDuration && meleeWeaponPivot.activeInHierarchy)
            {
                timer = Time.time - startTime;
                float yAxisAngle = Mathf.Lerp(minAngle, maxAngle, timer / attackDuration);
                meleeWeaponPivot.transform.localRotation = Quaternion.Euler(0, yAxisAngle, 0);
                // meleeWeapon.transform.localRotation = Quaternion.Lerp(startRotation, finalRotation, timer / attackDuration);
                yield return null;
            }

            meleeWeaponPivot.SetActive(false);
            meleeWeapon.GetComponent<MeleeWeapon>().ResetHittedEnemiesBuffer();
            yield return CoolDownCoroutine();
            _canAttack = true;
        }

        private IEnumerator CoolDownCoroutine()
        {
            yield return new WaitForSeconds(attackCoolDown);
        }
    }
}