using System.Collections;
using System.Collections.Generic;
using UnityEditor.EditorTools;
using UnityEngine;

public class Ball : MonoBehaviour
{
    int speed;
    Vector3 offset1;
    Vector3 offset2;
    Vector3 startPoint;
    Vector3 endPoint;
    public void Init(IPitchData iPitchData, int speed, Vector3 startPoint, Vector3 endPoint)
    {
        this.speed = speed;
        this.startPoint = startPoint;
        this.endPoint = endPoint;
        offset1 = startPoint+iPitchData.Offset1;
        offset2 = endPoint+ iPitchData.Offset2;        
    }

    public void Pitch()
    {
        StartCoroutine(ApplyPitchMovement(startPoint, offset1, offset2, endPoint));
    }

    IEnumerator ApplyPitchMovement(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float duration = 1.0f; // 총 이동 시간(초)
        float speedFactor = speed / 100.0f; // 속도 계수 (높을수록 빠름)

        // 총 이동 시간을 속도에 따라 조정
        duration = duration / speedFactor; // 속도가 높을수록 시간은 짧아짐

        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            // 경과 시간을 전체 시간으로 나누어 0~1 사이 값 계산
            float t = elapsedTime / duration;

            // 베지어 곡선을 따라 위치 계산
            gameObject.transform.position = BezierUtils.Bezier(p1, p2, p3, p4, t);


            elapsedTime += Time.deltaTime;

            yield return null;
        }

        gameObject.transform.position = p4;
        yield return new WaitForSeconds(2f);
        FindObjectOfType<ObjectPoolManager>().ReturnBall(this);

    }
}
