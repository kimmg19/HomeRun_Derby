using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-500)]
public class LobbyDependencyInjection : MonoBehaviour
{
    void Awake()
    {
        var playerManager = PlayerManager.Instance;

        var shopUIManager = FindObjectOfType<ShopUIManager>();
        if (shopUIManager != null)
        {
            shopUIManager.Initialize(playerManager);
        }
    }
}