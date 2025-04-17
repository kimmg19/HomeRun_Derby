using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI introText;
    [SerializeField] TextMeshProUGUI swingChanceText;
    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TextMeshProUGUI currentScoreText;
    [SerializeField] GameObject DataBox;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI pitchTypeText;
    [SerializeField] TextMeshProUGUI pitchPosText;

    int maxCount;
    void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount += ChangeCount;
            EventManager.Instance.OnGameStart += DisableIntro;
            EventManager.Instance.OnBallReleased += GetBallData;
            EventManager.Instance.EnableBallData += EnableBallData;
        }
        else Debug.LogError("HUD 이벤트 등록 실패");

    }
    void Start()
    {
        maxCount = HomerunDerbyManager.Instance != null ?
            HomerunDerbyManager.Instance.SwingCount : 15;
        DataBox.SetActive(false);
    }
    void ChangeCount(int count)
    {
        swingChanceText.text = $"{count} / {maxCount}";
    }
    void DisableIntro()
    {
        introText.enabled = false;
    }
    void GetBallData(float speed, EPitchPosition position, EPitchType type)
    {
        speedText.text = $"{speed:F1}KM";
        pitchPosText.text = position.ToString();
        pitchTypeText.text = type.ToString();
    }
    void EnableBallData(bool b)
    {
        DataBox.SetActive(b);
    }
    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount -= ChangeCount;
            EventManager.Instance.OnGameStart -= DisableIntro;
            EventManager.Instance.OnBallReleased -= GetBallData;
            EventManager.Instance.EnableBallData -= EnableBallData;
        }
        else Debug.LogError("HUD 이벤트 해제 실패");

    }
}
