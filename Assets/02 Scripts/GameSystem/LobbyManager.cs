using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    void Start()
    {


        Application.targetFrameRate = 60;

        SoundManager.Instance.PlayBGM(SoundManager.EBgm.Lobby);
    }
}
