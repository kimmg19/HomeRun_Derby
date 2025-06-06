using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]ParticleSystem particle;
    [SerializeField]TrailRenderer trailRenderer;
    public EPitchLocation PitchPosition { get; set; }
    int speed;
    [SerializeField]bool isTriggerd;
    Vector3 offset1;
    Vector3 offset2;
    Vector3 startPoint;
    Vector3 catchPoint;
    Coroutine activeCoroutine;

    

    public void Init(IPitchData iPitchData, EPitchLocation pitchPosition, int speed, Vector3 startPoint, Vector3 endPoint)
    {
        this.PitchPosition = pitchPosition;
        this.speed = speed;
        this.startPoint = startPoint;
        this.catchPoint = endPoint;
        offset1 = startPoint + iPitchData.Offset1;
        offset2 = endPoint + iPitchData.Offset2;
        isTriggerd = false;      
        particle.Stop(withChildren:false);
        trailRenderer.enabled = false;
    }

    public void Pitch()
    {
        // 기존 코루틴이 있다면 중지
        CheckCoroutine();
        activeCoroutine = StartCoroutine(ApplyPitchMovement(startPoint, offset1, offset2, catchPoint));
    }

    IEnumerator ApplyPitchMovement(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        
        float duration = 1.3f; // 총 이동 시간(초)
        float speedFactor = speed / 100.0f; // 속도 계수 (높을수록 빠름)
        // 약간의 랜덤 회전 적용
        Vector3 rotationAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        float rotationSpeed = speed * 20; // 속도에 비례한 회전 속도

        // 총 이동 시간을 속도에 따라 조정
        duration = duration / speedFactor; // 속도가 높을수록 시간은 짧아짐
        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            float t = elapsedTime / duration;

            // 베지어 곡선을 따라 이동 및 회전
            transform.position = BezierUtils.Bezier(p1, p2, p3, p4, t);
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // 최종 위치 보정
        transform.position = p4;
        // 잠시 대기 후 공 반환
        yield return new WaitForSeconds(2f);

        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.ReturnBall(this);
        }
    }

    public IEnumerator ApplyHitMovement(
        Vector3 startPoint,
        Vector3 offset1,
        Vector3 offset2,
        Vector3 endPoint)
    {

        // 기존 투구 코루틴 중지
        CheckCoroutine();
        gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        float duration = 3.0f;
        float elapsedTime = 0f;
        particle.Play(withChildren:true);
        trailRenderer.Clear();
        trailRenderer.enabled = true;
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
        if (ObjectPoolManager.Instance != null)
        {
            ObjectPoolManager.Instance.ReturnBall(this);
        }
    }
    void CheckCoroutine()
    {
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Zone") && !isTriggerd){

            isTriggerd = true;
            EventManager.Instance.PublishOnEnablePitchData(PitchPosition);
        }
    }
    void OnDisable()
    {
        CheckCoroutine();
    }
}