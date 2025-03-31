using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomerunDerbyManager : MonoBehaviour
{
    [SerializeField]PitcherManager pitcherManager;
    [SerializeField] float ballCount = 15;

    void Start()
    {
        StartCoroutine(StartPitching());
    }
    IEnumerator StartPitching()
    {
        while (ballCount > 0)
        {
            pitcherManager.Pitching();
            ballCount--;
            yield return new WaitForSeconds(12f);
        }
    }
}
