using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    // ½Ì±ÛÅæ ÆÐÅÏ ±¸Çö
    private static SoundManager instance;
    public static SoundManager Instance
    {
        get
        {
            if (instance == null)
            {
                var obj = FindAnyObjectByType<SoundManager>();
                if (obj != null)
                {
                    instance = obj;
                }
                else
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    static SoundManager Create()
    {
        return Instantiate(Resources.Load<SoundManager>("SoundManager"));
    }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);

    }
    public enum ESfx
    {
        Hit=0,
        Homerun,
        Pitching,
        Swing
    }
    public enum EBgm
    {
        Lobby=0,
        Crowd
    }
    [SerializeField] AudioClip[] sfxClips;
    [SerializeField] AudioClip[] bgmClips;

    [SerializeField] AudioSource sfxSource;
    [SerializeField] AudioSource bgmSource;


    public void PlaySFX(ESfx sfx, float volume = 1f)
    {        
        sfxSource.PlayOneShot(sfxClips[(int)sfx], volume);
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
