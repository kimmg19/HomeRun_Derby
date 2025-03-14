using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pitch", menuName = "PitchType")]
public class PitchTypeDataSO : ScriptableObject
{
    //enum��������.
    [SerializeField] PitchType pitchType;
    public float BaseSpeed { get; } = 100f;//��� ������ �⺻ �ӵ�.
    public float speedMultiplier = 1.4f;    //������ �⺻ �ӵ� ������.

    //so�� ������ ������̱� ������ ������Ƽ�� speed �� ����ϴ� �ź��� controller���� ������ ¥�� �������
    //public float speed;
    
}
