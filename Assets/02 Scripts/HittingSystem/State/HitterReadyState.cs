using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitterReadyState : IHitterState
{
    public void Enter(HitterManager hitter)
    {
        hitter.animator.SetTrigger("Ready");
    }

    /*public void Exit(HitterManager hitter)
    {
    }

    public void Update(HitterManager hitter)
    {

    }*/
}
