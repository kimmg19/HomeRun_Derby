using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New Pitch", menuName = "PitchType")]
public class PitchTypeDataSO : ScriptableObject
{
    //enum��������.
    [Header("���� ����")]
    [SerializeField] PitchType pitchType;
    public float BaseSpeed { get; } = 100f;//��� ������ �⺻ �ӵ�.
    public float speedMultiplier = 1.4f;    //������ �⺻ �ӵ� ������.
    public Vector3 offset1;
    public Vector3 offset2;


    //so�� ������ ������̱� ������ ������Ƽ�� speed �� ����ϴ� �ź��� controller���� ������ ¥�� �������
    //public float speed;

}
