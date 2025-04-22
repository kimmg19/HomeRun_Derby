using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI introText;         //��ġ�� ��ŸƮ �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI swingChanceText;   //���� ��ȸ
    [SerializeField] TextMeshProUGUI bestScoreText;     //�ְ�����
    [SerializeField] TextMeshProUGUI currentScoreText;  //��������
    [SerializeField] TextMeshProUGUI speedText;         //����
    [SerializeField] TextMeshProUGUI pitchTypeText;     //����
    [SerializeField] TextMeshProUGUI pitchPosText;      //������ġ
    [SerializeField] TextMeshProUGUI distanceText;      //��Ÿ�
    [SerializeField] TextMeshProUGUI homerunText;       //Ȩ�� ����
    [SerializeField] TextMeshProUGUI timingText;        //Ÿ�� Ÿ�̹�

    [SerializeField] GameObject pitchDataBox;           //���� ������ UI
    [SerializeField] GameObject hitDataBox;             //Ÿ�� ������ UI


    int maxCount;
    int bestScore;

    void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount += ChangeCount;
            EventManager.Instance.OnGameStart += DisableIntro;
            EventManager.Instance.OnBallReleased += GetPitchData;
            EventManager.Instance.EnableBallData += EnableBallData;
            EventManager.Instance.OnGameReady += InitializeUI;            
            EventManager.Instance.OnScoreChanged += UpdateScore;
            EventManager.Instance.OnHitResult += ShowHitResult;
        }
        else Debug.LogError("HUD �̺�Ʈ ��� ����");
    }
    void Awake()
    {
        if (introText == null) return;
        if (swingChanceText == null) return;
        if (bestScoreText == null) return;
        if (currentScoreText == null) return;
        if (speedText == null) return;
        if (pitchTypeText == null) return;
        if (pitchPosText == null) return;
        if (distanceText == null) return;
        if (homerunText == null) return;
        if (pitchDataBox == null) return;
        if (hitDataBox == null) return;
    }
    void Start()
    {
        maxCount = HomerunDerbyManager.Instance != null ?
            HomerunDerbyManager.Instance.SwingCount : 15;

        bestScore = PlayerPrefs.GetInt("BestScore", 0); // �ְ� ���� �ε�
        bestScoreText.text = $"{bestScore}";
        currentScoreText.text = "0";

        pitchDataBox.SetActive(false);
        hitDataBox.SetActive(false);
    }

    void InitializeUI()
    {
        // ���� ���۽� UI �ʱ�ȭ
        currentScoreText.text = "0";
        bestScoreText.text = $"{bestScore}";
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

    void GetPitchData(float speed, EPitchPosition position, EPitchType type)
    {
        speedText.text = $"{speed:F1}KM";
        pitchPosText.text = position.ToString();
        pitchTypeText.text = type.ToString();
    }

    void EnableBallData(bool b)
    {
        pitchDataBox.SetActive(b);
    }

    // ���� ���� ������Ʈ
    void UpdateScore(int score)
    {
        currentScoreText.text = $"{score}";
        if (score >= bestScore)
        {
            bestScoreText.text = $"{bestScore}";
        }
    }


    // Ÿ�� ��� ǥ��
    void ShowHitResult(bool isHomerun, float distance, EHitTiming timing)
    {
        distanceText.text = $"{distance:F1}m";
        timingText.text = timing.ToString();
        // Ȩ�� �˸� ǥ��
        if (isHomerun)
        {
            homerunText.color = Color.red;
            homerunText.text = "HOMERUN!!!!";
        }
        else
        {
            homerunText.color = Color.red;
            homerunText.text = "Hit!";
        }
        hitDataBox.SetActive(true);
        Invoke("HideHitDataText", 3.0f);        
    }

    void HideHitDataText()
    {
        hitDataBox.SetActive(false);
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount -= ChangeCount;
            EventManager.Instance.OnGameStart -= DisableIntro;
            EventManager.Instance.OnBallReleased -= GetPitchData;
            EventManager.Instance.EnableBallData -= EnableBallData;
            EventManager.Instance.OnGameReady -= InitializeUI;
            EventManager.Instance.OnScoreChanged -= UpdateScore;
            EventManager.Instance.OnHitResult -= ShowHitResult;
        }
        else Debug.LogError("HUD �̺�Ʈ ���� ����");
    }
}