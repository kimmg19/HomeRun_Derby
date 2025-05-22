using UnityEngine;

public class GameManager : MonoBehaviour
{
    static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }

    [SerializeField] int defaultFrameRate = 60;
    public int CurrentFrameRate { get; set; }

    const string FRAME_KEY = "FrameRate";

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Init()
    {
        // 설정 로드
        CurrentFrameRate = PlayerPrefs.GetInt(FRAME_KEY, defaultFrameRate);
        ApplyFrame();
    }

    void ApplyFrame()
    {
        Application.targetFrameRate = CurrentFrameRate;
    }

    public void SetFrameRate(int frameRate)
    {
        if (frameRate <= 0) return;

        CurrentFrameRate = frameRate;
        ApplyFrame();

        PlayerPrefs.SetInt(FRAME_KEY, CurrentFrameRate);
        PlayerPrefs.Save();
    }
}