using System.Collections;
using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI introText;         //��ġ�� ��ŸƮ �ؽ�Ʈ
    [SerializeField] TextMeshProUGUI swingChanceText;   //���� ��ȸ
    [SerializeField] TextMeshProUGUI bestScoreText;     //�� �ְ�����
    [SerializeField] TextMeshProUGUI currentScoreText;  //�� ��������
    [SerializeField] TextMeshProUGUI earnedScoreText;   //���� Ÿ������ ���� ����

    [SerializeField] TextMeshProUGUI speedText;         //����
    [SerializeField] TextMeshProUGUI pitchTypeText;     //����
    [SerializeField] TextMeshProUGUI pitchPosText;      //������ġ

    [SerializeField] TextMeshProUGUI distanceText;      //��Ÿ�
    [SerializeField] TextMeshProUGUI homerunText;       //Ȩ�� ����
    [SerializeField] TextMeshProUGUI timingText;        //Ÿ�� Ÿ�̹�
    [SerializeField] TextMeshProUGUI criticalText;      //ũ�� ����
    [SerializeField] TextMeshProUGUI bigText;

    [SerializeField] GameObject scoreBox;
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
            EventManager.Instance.OnSetBallData += GetPitchData;
            EventManager.Instance.OnEnablePitchData += ShowPitchData;
            EventManager.Instance.OnGameReady += InitializeUIAtStart;
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
        if (earnedScoreText == null) return;
        if (speedText == null) return;
        if (pitchTypeText == null) return;
        if (pitchPosText == null) return;
        if (distanceText == null) return;
        if (homerunText == null) return;
        if (pitchDataBox == null) return;
        if (hitDataBox == null) return;
        if (criticalText == null) return;
        if (scoreBox == null) return;
        if (bigText == null) return;
    }


    void InitializeUIAtStart()
    {
        // ���� ���۽� UI �ʱ�ȭ
        currentScoreText.text = "0";
        bestScoreText.text = $"{bestScore}";
        bestScore = PlayerPrefs.GetInt("BestScore", 0); // �ְ� ���� �ε�
        bestScoreText.text = $"{bestScore}";
        maxCount = HomerunDerbyManager.Instance != null ?
            HomerunDerbyManager.Instance.SwingCount : 15;
        pitchDataBox.SetActive(false);
        hitDataBox.SetActive(false);
        scoreBox.SetActive(false);
    }
    void InitDuringGame()
    {
        criticalText.text = "";
        bigText.text = "";
        earnedScoreText.text = "";
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

    void ShowPitchData(EPitchPosition p)
    {
        pitchDataBox.SetActive(true);
        StartCoroutine(HideHitDataText(pitchDataBox));
    }

    // ���� ���� ������Ʈ. ���� ����(Ÿ�� ����������), Ÿ�� �� ����, Ÿ������ ���� ���� ��.
    void UpdateScore(int currentScore, int targetScore)
    {
        if (currentScore >= bestScore)
        {
            StartCoroutine(ScoreEffect(bestScoreText, currentScore, targetScore));
        }
        StartCoroutine(ScoreEffect(currentScoreText, currentScore, targetScore));
    }
    IEnumerator ScoreEffect(TextMeshProUGUI scoreText, float currentScore, float targetScore)
    {
        float duration = 0.8f;
        float offset = (targetScore - currentScore) / duration;
        while (currentScore < targetScore)
        {
            currentScore += offset * Time.deltaTime;
            scoreText.text = ((int)currentScore).ToString();
            yield return null;
        }
        currentScore = targetScore;
        scoreText.text = ((int)currentScore).ToString();
    }

    // Ÿ�� ��� ǥ��
    void ShowHitResult(bool isHomerun, float distance, EHitTiming timing, int score, bool isCritical, bool isBig)
    {
        if (isCritical) criticalText.text = "+Critical!!";
        if (isBig) bigText.text = "+Big!!!";
        if (score > 0) earnedScoreText.text = $"+{score}";

        distanceText.text = $"{distance:F1}m";
        timingText.text = timing.ToString();

        // Ȩ�� �˸� ǥ��
        if (timing == EHitTiming.Miss)
        {
            homerunText.text = "OOPS!!";
        }
        else if (isHomerun)
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
        scoreBox.SetActive(true);
        StartCoroutine(HideHitDataText(scoreBox));
        StartCoroutine(HideHitDataText(hitDataBox));
    }

    IEnumerator HideHitDataText(GameObject box)
    {
        yield return new WaitForSeconds(3);
        box.SetActive(false);
        InitDuringGame();
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount -= ChangeCount;
            EventManager.Instance.OnGameStart -= DisableIntro;
            EventManager.Instance.OnSetBallData -= GetPitchData;
            EventManager.Instance.OnEnablePitchData -= ShowPitchData;
            EventManager.Instance.OnGameReady -= InitializeUIAtStart;
            EventManager.Instance.OnScoreChanged -= UpdateScore;
            EventManager.Instance.OnHitResult -= ShowHitResult;
        }
        else Debug.LogError("HUD �̺�Ʈ ���� ����");
    }
}