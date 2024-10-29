using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TESTPROTAGONIST : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Slider sliderX;

    public void Laser()
    {
        anim.SetFloat("BlendLaser", (sliderX.value * 2) - 1);
    }
    public void BombThrow()
    {
        anim.SetTrigger("BombThrow");
    }
    public void AttackUp()
    {
        anim.SetTrigger("AttackUp");
    }
    public void LeftRecovery()
    {
        anim.SetTrigger("LeftRecovery");
    }
    public void RightRecovery()
    {
        anim.SetTrigger("RightRecovery");
    }
    public void StartLaser()
    {
        anim.SetTrigger("StartLaser");
    }
    public void BossExplosion()
    {
        anim.SetTrigger("BossExplosion");
    }
    public void GetHit()
    {
        anim.SetTrigger("GetHit");
    }
    public void Death()
    {
        anim.SetTrigger("Death");
    }
    public void Recover()
    {
        anim.SetTrigger("Recover");
    }

}
