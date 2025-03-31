using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherReadyState : IPitcherState
{
    public void Enter(PitcherManager pitcher)
    {
        //Debug.Log("���� ���");
        pitcher.animator.SetTrigger("Ready");
    }

    public void Update(PitcherManager pitcher)
    {
        
    }

    public void Exit(PitcherManager pitcher)
    {
        
    }    
}
