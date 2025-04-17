using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PitcherWindUpState : IPitcherState
{
    public void Enter(PitcherManager pitcher)
    {
        // 와인드업 애니메이션 시작
        pitcher.animator.SetTrigger("WindUp");
    }

   /* public void Update(PitcherManager pitcher)
    {
        // 필요한 경우 업데이트 로직 구현
    }

    public void Exit(PitcherManager pitcher)
    {
        // 필요한 경우 종료 로직 구현
    }*/
}