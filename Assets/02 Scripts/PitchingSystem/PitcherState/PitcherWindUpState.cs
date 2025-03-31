using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherWindUpState : IPitcherState
{
    public void Enter(PitcherManager pitcher)
    {
        //Debug.Log("투수 와인드업");
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
