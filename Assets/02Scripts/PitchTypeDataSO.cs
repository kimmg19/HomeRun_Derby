using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pitch", menuName = "PitchType")]
public class PitchTypeDataSO : ScriptableObject
{
    //enum가져오기.
    [SerializeField] PitchType pitchType;
    public float BaseSpeed { get; } = 100f;//모든 구종의 기본 속도.
    public float speedMultiplier = 1.4f;    //구종별 기본 속도 설정값.

    //so는 데이터 저장소이기 때문에 프로퍼티로 speed 를 사용하는 거보다 controller에서 로직을 짜는 방식으로
    //public float speed;
    
}
