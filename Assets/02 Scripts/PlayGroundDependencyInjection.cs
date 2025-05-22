using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-500)]
public class PlayGroundDependencyInjection : MonoBehaviour
{
    void Awake()
    {
        var playerManager = PlayerManager.Instance;

        var hitterManager = FindObjectOfType<HitterManager>();
        if (hitterManager != null)
        {
            hitterManager.Initialize(playerManager);
        }
    }
}
