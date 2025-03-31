using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherWindUpState : IPitcherState
{
    public void Enter(PitcherManager pitcher)
    {
        //Debug.Log("���� ���ε��");
        pitcher.animator.SetTrigger("WindUp");
        pitcher.onWindUpStart?.Invoke();
    }

    public void Update(PitcherManager pitcher)
    {

    }

    public void Exit(PitcherManager pitcher)
    {

    }
}
