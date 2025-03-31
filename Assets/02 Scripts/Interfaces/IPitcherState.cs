using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPitcherState
{
    void Enter(PitcherManager pitcher);
    void Update(PitcherManager pitcher);
    void Exit(PitcherManager pitcher);
}
