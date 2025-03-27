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
        float duration = 1.0f; // �� �̵� �ð�(��)
        float speedFactor = speed / 100.0f; // �ӵ� ��� (�������� ����)

        // �� �̵� �ð��� �ӵ��� ���� ����
        duration = duration / speedFactor; // �ӵ��� �������� �ð��� ª����

        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            // ��� �ð��� ��ü �ð����� ������ 0~1 ���� �� ���
            float t = elapsedTime / duration;

            // ������ ��� ���� ��ġ ���
            gameObject.transform.position = BezierUtils.Bezier(p1, p2, p3, p4, t);


            elapsedTime += Time.deltaTime;

            yield return null;
        }

        gameObject.transform.position = p4;
        yield return new WaitForSeconds(2f);
        FindObjectOfType<ObjectPoolManager>().ReturnBall(this);

    }
}
