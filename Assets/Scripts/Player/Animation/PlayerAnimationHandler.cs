using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private PlayerAnimationConfigSO animationConfig;

    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetPlayerDirection(Vector2 normalizedDir)
    {
        _animator.SetFloat("velocity_x", normalizedDir.x, animationConfig.moveDampTime, Time.deltaTime);
        _animator.SetFloat("velocity_z", normalizedDir.y, animationConfig.moveDampTime, Time.deltaTime);
    }
}