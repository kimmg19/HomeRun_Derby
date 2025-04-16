using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pitch", menuName = "HomerunDerby/PitchType")]
public class PitchTypeDataSO : ScriptableObject, IPitchData
{
    
    [Header("���� ����")]
    public EPitchType pitchType;
    [Tooltip("���� ����")]
    [SerializeField] float speedMultiplier = 1.4f;
    [Tooltip("ù ��° ������")]
    [SerializeField] Vector3 offset1; 
    [Tooltip("�� ��° ������")]
    [SerializeField] Vector3 offset2; 

    private const float DEFAULT_SPEED = 100f;
    public float BaseSpeed => DEFAULT_SPEED;

    public EPitchType Type => pitchType;
    public float SpeedMultiplier => speedMultiplier;
    public Vector3 Offset1 => offset1;
    public Vector3 Offset2 => offset2;

}
