using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPitchData {
     PitchType Type { get; }
     float BaseSpeed { get; } //모든 구종의 기본 속도.
     float SpeedMultiplier { get; }     //구종별 기본 속도 설정값.
     Vector3 Offset1 { get; } //투구 궤적 offset1
     Vector3 Offset2 { get; }  //투구 궤적 offset2
}

