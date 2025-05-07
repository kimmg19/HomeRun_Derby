using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    void Start()
    {
        SoundManager.Instance.PlayBGM(SoundManager.EBgm.Lobby);
    }
}
