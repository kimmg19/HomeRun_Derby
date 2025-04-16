using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager Instance;

    [SerializeField] Ball ballPrefab;
    [SerializeField] int initialPoolSize = 5;
    [SerializeField] Transform pitchingPoint;

    Queue<Ball> poolQueue = new Queue<Ball>();

    void Awake()
    {
        Instance = this;
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            Ball b = Instantiate(ballPrefab);
            b.gameObject.SetActive(false);
            b.transform.SetParent(pitchingPoint);
            poolQueue.Enqueue(b);
        }
    }

    public Ball GetBall()
    {
        Ball ball;

        if (poolQueue.Count == 0)
        {
            // 필요시 새 공 생성
            ball = Instantiate(ballPrefab, pitchingPoint.position, Quaternion.identity);
        }
        else
        {
            ball = poolQueue.Dequeue();
            ball.transform.position = pitchingPoint.position;
            ball.gameObject.SetActive(true);
        }

        ball.transform.SetParent(pitchingPoint);
        return ball;
    }

    public void ReturnBall(Ball ball)
    {
        if (ball == null) return;

        ball.gameObject.SetActive(false);
        poolQueue.Enqueue(ball);
    }
}