using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    PitchTypeDataSO pitchTypeDataSO;
    int speed;
    public void Init(PitchTypeDataSO pitchTypeDataSO,int speed)
    {
        this.pitchTypeDataSO = pitchTypeDataSO;
        this.speed = speed; 
        ApplyPitchMovement();
    }
    void ApplyPitchMovement()
    {
        MoveTo();
    }
    void MoveTo()
    {

    }
}
