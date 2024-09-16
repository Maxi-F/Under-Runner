using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEST_MINION_A : MonoBehaviour
{
    [SerializeField] Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Aim()
    {
        animator.SetTrigger("AIM");
    }
    public void PrepareAttack()
    {
        animator.SetTrigger("PREPARE_ATTACK");
    }
    public void Attack()
    {
        animator.SetTrigger("ATTACK");
    }
    public void Hit()
    {
        animator.SetTrigger("GET_HIT");
    }
}
