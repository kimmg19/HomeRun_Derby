using UnityEngine;

public class SoundManager : MonoBehaviour
{
    private static SoundManager instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public static SoundManager Instance
    {
        get
        {
            if (instance == null) return null;
            return instance;
        }
    }
    public enum ESfx
    {
        Hit
    }
    public enum EBgm
    {
        Lobby
    }
    [SerializeField] AudioClip[] sfxClips;
    [SerializeField] AudioClip[] bgmClips;

    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource bgmSource;


    public void PlaySFX(ESfx sfx)
    {        
        sfxSource.PlayOneShot(sfxClips[(int)sfx],1f);
    }

    public void PlayBGM(EBgm bgm)
    {
        bgmSource.clip = bgmClips[(int)bgm];
        bgmSource.Play();
    }
    public void StopBgm()
    {
        bgmSource.Stop();
    }
}
