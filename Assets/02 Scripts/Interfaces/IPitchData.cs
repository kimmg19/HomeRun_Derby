using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPitchData {
     PitchType Type { get; }
     float BaseSpeed { get; } //��� ������ �⺻ �ӵ�.
     float SpeedMultiplier { get; }     //������ �⺻ �ӵ� ������.
     Vector3 Offset1 { get; } //���� ���� offset1
     Vector3 Offset2 { get; }  //���� ���� offset2
}

