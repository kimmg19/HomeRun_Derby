using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtBallCamera : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera baseCamera;
    [SerializeField] CinemachineVirtualCamera followCamera;
    [SerializeField] int defaultPriority = 1;
    [SerializeField] int activePriority = 20;
    [SerializeField] float followDuration = 1f;
    void OnEnable()
    {
        EventManager.Instance.OnBallHit += ActiveFollowCamera;
    }
    //타이밍만 사용.
    void ActiveFollowCamera(EHitTiming timing, float f)
    {
        Ball ball=HomerunDerbyManager.Instance.CurrentBall;
        followCamera.LookAt = ball.transform;
        followCamera.Priority = activePriority;
        StartCoroutine(ResetCameraPriority());
    }
    IEnumerator ResetCameraPriority()
    {
        yield return new WaitForSeconds(followDuration);
        followCamera.Priority = defaultPriority;

    }
    void OnDisable()
    {
        EventManager.Instance.OnBallHit -= ActiveFollowCamera;

    }
}
