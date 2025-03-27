using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HitterManager : MonoBehaviour
{
    [SerializeField]Animator animator;
    PitchingManager pm;
    void Awake()
    {
        pm=FindObjectOfType<PitchingManager>();
        pm.ad += OnSwingReady;
    }
    public void OnSwingReady()
    {
        print("음");
        animator.SetTrigger("HitReady");
        animator.ResetTrigger("Hit");
    }
    //아마도 투수 던질 때랑 타자 힛레디 할 때 이벤트 처리해서 같이 실행시키는게 좋을 듯
    void OnSwing()
    {
        animator.SetTrigger("Hit");
    }
    void OnDisable()
    {
        pm.ad -= OnSwingReady;
    }
}
