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
        // Ǯ �Ŵ��� ������ ĳ���Ͽ� ���� ����
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
        // ���� �ڷ�ƾ�� �ִٸ� ����
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
        }

        activeCoroutine = StartCoroutine(ApplyPitchMovement(startPoint, offset1, offset2, catchPoint));
    }

    IEnumerator ApplyPitchMovement(Vector3 p1, Vector3 p2, Vector3 p3, Vector3 p4)
    {
        float duration = 1.3f; // �� �̵� �ð�(��)
        float speedFactor = speed / 100.0f; // �ӵ� ��� (�������� ����)

        // �� �̵� �ð��� �ӵ��� ���� ����
        duration = duration / speedFactor; // �ӵ��� �������� �ð��� ª����
        float elapsedTime = 0f;

        while (elapsedTime <= duration)
        {
            // ��� �ð��� ��ü �ð����� ������ 0~1 ���� �� ���
            float t = elapsedTime / duration;

            // ������ ��� ���� ��ġ ���
            transform.position = BezierUtils.Bezier(p1, p2, p3, p4, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ��ġ ����
        transform.position = p4;

        // ��� ��� �� �� ��ȯ
        yield return new WaitForSeconds(2f);

        if (poolManager != null)
        {
            poolManager.ReturnBall(this);
        }
        else
        {
            // Ǯ �Ŵ����� ã�� ������ ��� ���� ã��
            
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
        // ���� ���� �ڷ�ƾ ����
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

            // ������ ��� ���� ��ġ ���
            transform.position = BezierUtils.Bezier(startPoint, offset1, offset2, endPoint, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // ���� ��ġ ����
        transform.position = endPoint;

        // �� ��ȯ
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
        // �ڷ�ƾ ����
        if (activeCoroutine != null)
        {
            StopCoroutine(activeCoroutine);
            activeCoroutine = null;
        }
    }
}