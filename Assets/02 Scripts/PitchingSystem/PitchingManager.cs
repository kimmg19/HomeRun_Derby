using UnityEngine;

public class PitchingManager : MonoBehaviour
{
    [Header("�� ������")]
    [SerializeField] Ball ball;
    [SerializeField] PitchTypeDataSO[] pitchTypeDataSO;

    [Header("���� �Ӽ�")]
    [SerializeField] Transform pitcherRightHand; //������ ��.    

    [Header("��Ʈ����ũ Ȯ��")]
    [SerializeField] float strikeChance = 0.7f;

    [Header("���� ���̵�")]
    [SerializeField] int currentDifficulty = 1;//���̵��� ���� ���� ����. ����� 1~3�ܰ�

    StrikeZoneManager strikeZoneManager;

    public enum PitchPosition
    {
        Strike,
        Ball
    }
    void Awake()
    {
        strikeZoneManager = GetComponent<StrikeZoneManager>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartPitching(pitchTypeDataSO[Random.Range(0, pitchTypeDataSO.Length)], currentDifficulty);//�켱�� ��ü ����.
        }
    }

    //���� ����.���̵��� ���� ���� ����.    
    void StartPitching(IPitchData pitchData, int currentDifficulty)
    {
        Debug.Log("���� ����");
        PitchPosition pitchPosition = Random.value < strikeChance ? PitchPosition.Strike : PitchPosition.Ball;         //Strike or Ball ���ϱ�
        Vector3 endPoint = strikeZoneManager.SetPitchingPoint(pitchPosition);

        float finalSpeed = SetBallSpeed(pitchData, currentDifficulty);//ui�� ����-�Ҽ��� ù° �ڸ����� ǥ�� ����

        ball = Instantiate(ball, pitcherRightHand.position, Quaternion.identity);        //������Ʈ Ǯ ���� ����
        ball.Init(pitchData, (int)finalSpeed, pitcherRightHand.position, endPoint);
    }
    //���� ����
    float SetBallSpeed(IPitchData pitchData, int currentDifficulty)
    {
        float baseMultiplier = pitchData.SpeedMultiplier;
        float baseSpeed = pitchData.BaseSpeed;
        float difficultyMultiplier = 1f + (currentDifficulty - 1) * 0.05f;    //0%, 5%, 10%
        float speed = Random.Range(baseSpeed * baseMultiplier,
            baseSpeed * baseMultiplier + 10);

        return speed * difficultyMultiplier;
    }
}
