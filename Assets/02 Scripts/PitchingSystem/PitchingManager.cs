using UnityEngine;

public class PitchingManager : MonoBehaviour
{
    [Header("공 프리펩")]
    [SerializeField] Ball ball;
    [SerializeField] PitchTypeDataSO[] pitchTypeDataSO;

    [Header("투구 속성")]
    [SerializeField] Transform pitcherRightHand; //투수의 손.    

    [Header("스트라이크 확률")]
    [SerializeField] float strikeChance = 0.7f;

    [Header("현재 난이도")]
    [SerializeField] int currentDifficulty = 1;//난이도에 따라 구속 조절. 현재는 1~3단계

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
            StartPitching(pitchTypeDataSO[Random.Range(0, pitchTypeDataSO.Length)], currentDifficulty);//우선은 전체 랜덤.
        }
    }

    //구종 설정.난이도에 따른 구속 설정.    
    void StartPitching(IPitchData pitchData, int currentDifficulty)
    {
        Debug.Log("투구 시작");
        PitchPosition pitchPosition = Random.value < strikeChance ? PitchPosition.Strike : PitchPosition.Ball;         //Strike or Ball 정하기
        Vector3 endPoint = strikeZoneManager.SetPitchingPoint(pitchPosition);

        float finalSpeed = SetBallSpeed(pitchData, currentDifficulty);//ui용 구속-소수점 첫째 자리까지 표현 위해

        ball = Instantiate(ball, pitcherRightHand.position, Quaternion.identity);        //오브젝트 풀 적용 예정
        ball.Init(pitchData, (int)finalSpeed, pitcherRightHand.position, endPoint);
    }
    //구속 설정
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
