using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherThrowState : IPitcherState
{
    public void Enter(PitcherManager pitcher)
    {
        pitcher.eState = PitchState.Throw;

        //Debug.Log("≈ı±∏");
        pitcher.ExecutePitch();
    }

    public void Update(PitcherManager pitcher)
    {

    }

    public void Exit(PitcherManager pitcher)
    {        

    }
}
