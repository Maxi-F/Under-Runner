using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minion_b_tests : MonoBehaviour
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
    public void HitFront()
    {
        animator.SetTrigger("player_hits");
    }
    public void HitBack()
    {
        animator.SetTrigger("boss_hits");
    }
}
