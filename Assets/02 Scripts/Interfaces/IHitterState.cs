using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHitterState
{
    void Enter(HitterManager hitter);
    void Update(HitterManager hitter);  
    void Exit(HitterManager hitter);
}
