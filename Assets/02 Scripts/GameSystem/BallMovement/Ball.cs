using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    TrailRenderer trailRenderer;
    public EPitchPosition PitchPosition { get; set; }
    int speed;
    Vector3 offset1;
    Vector3 offset2;
    Vector3 startPoint;
    Vector3 catchPoint;
    Coroutine activeCoroutine;

    void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();        
    }

    public void Init(IPitchData iPitchData, EPitchPosition pitchPosition, int speed, Vector3 startPoint, Vector3 endPoint)
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
        // ���� �ڷ�ƾ�� �ִٸ� ����
        CheckCoroutine();
        activeCoroutine = StartCoroutine(ApplyPitchMovement(startPoint, offset1, offset2, catchPoint));
    }

    IEnumerator ApplyPitchMovement(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        trailRenderer.enabled = false;
        float duration = 1.3f; // �� �̵� �ð�(��)
        float speedFactor = speed / 100.0f; // �ӵ� ��� (�������� ����)
        // �ణ�� ���� ȸ�� ����
        Vector3 rotationAxis = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
        float rotationSpeed = speed * 20; // �ӵ��� ����� ȸ�� �ӵ�

        // �� �̵� �ð��� �ӵ��� ���� ����
        duration = duration / speedFactor; // �ӵ��� �������� �ð��� ª����
        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            float t = elapsedTime / duration;

            // ������ ��� ���� �̵� �� ȸ��
            transform.position = BezierUtils.Bezier(p1, p2, p3, p4, t);
            transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // ���� ��ġ ����
        transform.position = p4;
        EventManager.Instance.PublishEnableBallData(true);
        // ��� ��� �� �� ��ȯ
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
        // ���� ���� �ڷ�ƾ ����
        CheckCoroutine();
        float duration = 3.0f;
        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            if(elapsedTime>=0.1f)trailRenderer.enabled = true;
            float t = elapsedTime / duration;

            // ������ ��� ���� ��ġ ���
            transform.position = BezierUtils.Bezier(startPoint, offset1, offset2, endPoint, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ��ġ ����
        transform.position = endPoint;
        trailRenderer.enabled = false;
        // �� ��ȯ
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
    void OnDisable()
    {
        CheckCoroutine();
    }
}