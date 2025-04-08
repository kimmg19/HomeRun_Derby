using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public EPitchPosition PitchPosition {  get; private set; }
    int speed;
    Vector3 offset1;
    Vector3 offset2;
    Vector3 startPoint;
    Vector3 catchPoint;
    Coroutine activeCoroutine;
    ObjectPoolManager poolManager;

    void Awake()
    {
        // 풀 매니저 참조를 캐싱하여 성능 개선
        poolManager = FindObjectOfType<ObjectPoolManager>();
    }

    public void Init(IPitchData iPitchData, EPitchPosition pitchPosition,int speed, Vector3 startPoint, Vector3 endPoint)
    {
        this.PitchPosition = pitchPosition;
        this.speed = speed;
        this.startPoint = startPoint;
        this.catchPoint = endPoint;
        offset1 = startPoint + iPitchData.Offset1;
        offset2 = endPoint + iPitchData.Offset2;
    }

    public void Pitch()
    {
        // 기존 코루틴이 있다면 중지
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        activeCoroutine = StartCoroutine(ApplyPitchMovement(startPoint, offset1, offset2, catchPoint));
    }

    IEnumerator ApplyPitchMovement(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float duration = 1.3f; // 총 이동 시간(초)
        float speedFactor = speed / 100.0f; // 속도 계수 (높을수록 빠름)

        // 총 이동 시간을 속도에 따라 조정
        duration = duration / speedFactor; // 속도가 높을수록 시간은 짧아짐
        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            // 경과 시간을 전체 시간으로 나누어 0~1 사이 값 계산
            float t = elapsedTime / duration;

            // 베지어 곡선을 따라 위치 계산
            transform.position = BezierUtils.Bezier(p1, p2, p3, p4, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치 보정
        transform.position = p4;

        // 잠시 대기 후 공 반환
        yield return new WaitForSeconds(2f);

        if (poolManager != null)
        {
            poolManager.ReturnBall(this);
        }
        else
        {
            // 풀 매니저를 찾지 못했을 경우 직접 찾기
            
            if (poolManager != null)
            {
                poolManager.ReturnBall(this);
            }
        }
    }

    public IEnumerator ApplyHitMovement(
        Vector3 startPoint,
        Vector3 offset1,
        Vector3 offset2,
        Vector3 endPoint)
    {
        // 기존 투구 코루틴 중지
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }

        float duration = 3.0f;
        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {            
            float t = elapsedTime / duration;

            // 베지어 곡선을 따라 위치 계산
            transform.position = BezierUtils.Bezier(startPoint, offset1, offset2, endPoint, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // 최종 위치 보정
        transform.position = endPoint;

        // 공 반환
        if (poolManager != null)
        {
            poolManager.ReturnBall(this);
        }
        else
        {
            if (poolManager != null)
            {
                poolManager.ReturnBall(this);
            }
        }
    }

    void OnDisable()
    {
        // 코루틴 정리
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
    }
}