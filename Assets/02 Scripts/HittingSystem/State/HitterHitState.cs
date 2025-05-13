using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitterHitState : IHitterState
{
    public void Enter(HitterManager hitter)
    {
        //Debug.LogError("Swing");
        hitter. animator.SetTrigger("Hit");
    }

    /*public void Exit(HitterManager hitter)
    {
    }

    public void Update(HitterManager hitter)
    {
    }*/


}
