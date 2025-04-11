using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    
    [SerializeField] Ball ballPrefab;  // 원본 프리팹 참조명 변경
    [SerializeField] int initialPoolSize = 1;  // 초기 풀 크기
    [SerializeField] Transform pitchingPoint;
    Queue<Ball> poolQueue = new Queue<Ball>();

    void Awake()
    {
        
        // 초기에 풀 채우기
        InitializePool();
    }

    void InitializePool()
    {
        for (int i = 0; i < initialPoolSize; i++)
        {
            Ball b = Instantiate(ballPrefab);
            b.gameObject.SetActive(false);
            b.transform.SetParent(pitchingPoint);  // 풀 매니저를 부모로 설정
            poolQueue.Enqueue(b);
        }
    }

    public Ball GetBall()
    {
        Ball qBall;

        if (poolQueue.Count == 0)
        {
            Debug.Log("풀이 비어 새로운 볼 생성");
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