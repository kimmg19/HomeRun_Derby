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
        print("��");
        animator.SetTrigger("HitReady");
        animator.ResetTrigger("Hit");
    }
    //�Ƹ��� ���� ���� ���� Ÿ�� ������ �� �� �̺�Ʈ ó���ؼ� ���� �����Ű�°� ���� ��
    void OnSwing()
    {
        animator.SetTrigger("Hit");
    }
    void OnDisable()
    {
        pm.ad -= OnSwingReady;
    }
}
