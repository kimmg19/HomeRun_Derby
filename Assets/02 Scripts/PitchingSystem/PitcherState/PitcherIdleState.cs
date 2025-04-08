using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherIdleState : IPitcherState
{
    public void Enter(PitcherManager pitcher)
    {
        
        pitcher.eState=PitchState.Ready;
        pitcher.animator.ResetTrigger("Ready");
        pitcher.animator.ResetTrigger("WindUp");
    }

    public void Exit(PitcherManager pitcher)
    {
    }

    public void Update(PitcherManager pitcher)
    {
    }

    
}
