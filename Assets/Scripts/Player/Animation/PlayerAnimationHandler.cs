using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationHandler : MonoBehaviour
{
    [SerializeField] private float movementDampTime;
    
    private Animator _animator;
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void SetPlayerDirection(Vector2 normalizedDir)
    {
        _animator.SetFloat("velocity_x", normalizedDir.x, movementDampTime, Time.deltaTime);
        _animator.SetFloat("velocity_z", normalizedDir.y, movementDampTime, Time.deltaTime);
    }
}