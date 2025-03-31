using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    
    [SerializeField] Ball ballPrefab;  // ���� ������ ������ ����
    [SerializeField] int initialPoolSize = 1;  // �ʱ� Ǯ ũ��
    [SerializeField] Transform pitchingPoint;
    Queue<Ball> poolQueue = new Queue<Ball>();

    void Awake()
    {
        
        // �ʱ⿡ Ǯ ä���
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            Ball b = Instantiate(ballPrefab);
            b.gameObject.SetActive(false);
            b.transform.SetParent(pitchingPoint);  // Ǯ �Ŵ����� �θ�� ����
            poolQueue.Enqueue(b);
        }
    }

    public Ball GetBall()
    {
        Ball qBall;

        if (poolQueue.Count == 0)
        {
            Debug.Log("Ǯ�� ��� ���ο� �� ����");
            qBall = Instantiate(ballPrefab, pitchingPoint.position, Quaternion.identity);
        }
        else
        {            
            qBall = poolQueue.Dequeue();
            qBall.transform.position = pitchingPoint.position;            
            qBall.gameObject.SetActive(true);
        }

        qBall.transform.SetParent(pitchingPoint);
        return qBall;
    }

    public void ReturnBall(Ball ball)
    {
        ball.gameObject.SetActive(false);
        poolQueue.Enqueue(ball);
    }
}