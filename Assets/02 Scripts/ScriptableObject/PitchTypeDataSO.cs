using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pitch", menuName = "HomerunDerby/PitchType")]
public class PitchTypeDataSO : ScriptableObject, IPitchData
{
    
    [Header("구종 설정")]
    public EPitchType pitchType;
    [Tooltip("구속 배율")]
    [SerializeField] float speedMultiplier = 1.4f;
    [Tooltip("첫 번째 제어점")]
    [SerializeField] Vector3 offset1; 
    [Tooltip("두 번째 제어점")]
    [SerializeField] Vector3 offset2; 

    private const float DEFAULT_SPEED = 100f;
    public float BaseSpeed => DEFAULT_SPEED;

    public EPitchType Type => pitchType;
    public float SpeedMultiplier => speedMultiplier;
    public Vector3 Offset1 => offset1;
    public Vector3 Offset2 => offset2;

}
