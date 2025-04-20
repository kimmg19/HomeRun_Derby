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
    [SerializeField] GameObject homerunText; // Ȩ�� �˸� UI (���û���)
    [SerializeField] TextMeshProUGUI distanceText; // �Ÿ� ǥ�ÿ� �ؽ�Ʈ (���û���)

    int maxCount;
    int bestScore;

    void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount += ChangeCount;
            EventManager.Instance.OnGameStart += DisableIntro;
            EventManager.Instance.OnBallReleased += GetBallData;
            EventManager.Instance.EnableBallData += EnableBallData;
            EventManager.Instance.OnGameReady += InitializeUI;

            // ���� ���� �̺�Ʈ ����
            EventManager.Instance.OnScoreChanged += UpdateScore;
            EventManager.Instance.OnBestScoreChanged += UpdateBestScore;
            EventManager.Instance.OnHomerunResult += ShowHomerunResult;
        }
        else Debug.LogError("HUD �̺�Ʈ ��� ����");
    }

    void Start()
    {
        maxCount = HomerunDerbyManager.Instance != null ?
            HomerunDerbyManager.Instance.SwingCount : 15;
        DataBox.SetActive(false);

        // �ְ� ���� �ε�
        bestScore = PlayerPrefs.GetInt("BestScore", 0);
        if (bestScoreText != null)
            bestScoreText.text = $"{bestScore}";

        if (currentScoreText != null)
            currentScoreText.text = "0";

        if (homerunText != null)
            homerunText.SetActive(false);
    }

    void InitializeUI()
    {
        // ���� ���۽� UI �ʱ�ȭ
        if (currentScoreText != null)
            currentScoreText.text = "0";

        if (bestScoreText != null)
            bestScoreText.text = $"{bestScore}";

        if (distanceText != null)
            distanceText.text = "";
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

    // ���� ���� ������Ʈ
    void UpdateScore(int score)
    {
        if (currentScoreText != null)
            currentScoreText.text = $"{score}";
    }

    // �ְ� ���� ������Ʈ
    void UpdateBestScore(int score)
    {
        bestScore = score;
        if (bestScoreText != null)
            bestScoreText.text = $"{bestScore}";
    }

    // Ȩ�� ��� ǥ��
    void ShowHomerunResult(bool isHomerun, float distance)
    {
        // �Ÿ� ǥ��
        if (distanceText != null)
            distanceText.text = $"{distance:F1}m";

        // Ȩ�� �˸� ǥ��
        if (homerunText != null && isHomerun)
        {
            homerunText.SetActive(true);
            Invoke("HideHomerunText", 2.0f);
        }
    }

    void HideHomerunText()
    {
        if (homerunText != null)
            homerunText.SetActive(false);
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount -= ChangeCount;
            EventManager.Instance.OnGameStart -= DisableIntro;
            EventManager.Instance.OnBallReleased -= GetBallData;
            EventManager.Instance.EnableBallData -= EnableBallData;
            EventManager.Instance.OnGameReady -= InitializeUI;

            // ���� ���� �̺�Ʈ ���� ����
            EventManager.Instance.OnScoreChanged -= UpdateScore;
            EventManager.Instance.OnBestScoreChanged -= UpdateBestScore;
            EventManager.Instance.OnHomerunResult -= ShowHomerunResult;
        }
        else Debug.LogError("HUD �̺�Ʈ ���� ����");
    }
}