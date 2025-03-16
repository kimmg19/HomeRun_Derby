using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    PitchTypeDataSO pitchTypeDataSO;
    int speed;
    Vector3 offset1;
    Vector3 offset2;
    public void Init(PitchTypeDataSO pitchTypeDataSO, int speed, Vector3 startPoint, Vector3 endPoint)
    {
        this.pitchTypeDataSO = pitchTypeDataSO;
        this.speed = speed;
        offset1 = pitchTypeDataSO.offset1;
        offset2 = pitchTypeDataSO.offset2;

        StartCoroutine(ApplyPitchMovement(startPoint,offset1,offset2,endPoint));
    }
    IEnumerator ApplyPitchMovement(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float duration = 1.0f; // �� �̵� �ð�(��)
        float speedFactor = speed / 100.0f; // �ӵ� ��� (�������� ����)

        // �� �̵� �ð��� �ӵ��� ���� ����
        duration = duration / speedFactor; // �ӵ��� �������� �ð��� ª����

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // ��� �ð��� ��ü �ð����� ������ 0~1 ���� �� ���
            float t = elapsedTime / duration;

            // ������ ��� ���� ��ġ ���
            gameObject.transform.position = BezierUtils.Bezier(p1, p2, p3, p4,t);

            // �ð� ������Ʈ
            elapsedTime += Time.deltaTime;

            yield return null;
        }
        
        gameObject.transform.position = p4;
    }

}
