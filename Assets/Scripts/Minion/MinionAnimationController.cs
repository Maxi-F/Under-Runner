using UnityEngine;

namespace Minion
{
    public class MinionAnimationController : MonoBehaviour
    {
        [SerializeField] Animator animator;

        [Header("Animations")] 
        [SerializeField] private string aim = "AIM";
        [SerializeField] private string prepareAttack = "PREPARE_ATTACK";
        [SerializeField] private string attack = "ATTACK";
        [SerializeField] private string getHit = "GET_HIT";

    
        public void Aim()
        {
            animator.SetTrigger(aim);
        }
    
        public void PrepareAttack()
        {
            animator.SetTrigger(prepareAttack);
        }
    
        public void Attack()
        {
            animator.SetTrigger(attack);
        }
    
        public void Hit()
        {
            animator.SetTrigger(getHit);
        }
    }
}
