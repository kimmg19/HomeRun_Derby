using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitterHitReadyState : IHitterState
{
    public void Enter(HitterManager hitter)
    {
        hitter.animator.SetTrigger("HitReady");
        hitter.animator.ResetTrigger("Hit");
    }

    public void Exit(HitterManager hitter)
    {
    }

    public void Update(HitterManager hitter)
    {
    }

    
}
