using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherWindUpState : IPitcherState
{
    public void Enter(PitcherManager pitcher)
    {
        // ���ε�� �ִϸ��̼� ����
        pitcher.animator.SetTrigger("WindUp");
    }

   /* public void Update(PitcherManager pitcher)
    {
        // �ʿ��� ��� ������Ʈ ���� ����
    }

    public void Exit(PitcherManager pitcher)
    {
        // �ʿ��� ��� ���� ���� ����
    }*/
}