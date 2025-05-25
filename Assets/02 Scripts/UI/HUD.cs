using System.Collections;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    StringBuilder sb = new StringBuilder(32);
    [Header("�ý��� �ؽ�Ʈ")]
    [SerializeField] TextMeshProUGUI swingChanceText;   //���� ��ȸ
    [SerializeField] TextMeshProUGUI bestScoreText;     //�� �ְ�����
    [SerializeField] TextMeshProUGUI currentScoreText;  //�� ��������
    [SerializeField] TextMeshProUGUI earnedScoreText;   //���� Ÿ������ ���� ����
    [SerializeField] TextMeshProUGUI finalScoreText;    // Ÿ������ ���� �� ����
    [SerializeField] TextMeshProUGUI earnedCurrencyText;// ���� ��ȭ

    [Header("���� ����")]
    [SerializeField] TextMeshProUGUI speedText;         //����
    [SerializeField] TextMeshProUGUI pitchTypeText;     //����
    [SerializeField] TextMeshProUGUI pitchPosText;      //������ġ

    [Header("Ÿ�� ����")]
    [SerializeField] TextMeshProUGUI distanceText;      //��Ÿ�
    [SerializeField] TextMeshProUGUI homerunText;       //Ȩ�� ����
    [SerializeField] TextMeshProUGUI timingText;        //Ÿ�� Ÿ�̹�
    [SerializeField] TextMeshProUGUI criticalText;      //ũ�� ����
    [SerializeField] TextMeshProUGUI bigText;

    [Header("Ÿ�� ����")]
    [SerializeField] TextMeshProUGUI powerStat;
    [SerializeField] TextMeshProUGUI judgeStat;
    [SerializeField] TextMeshProUGUI criticalStat;


    [Header("���� �� Ÿ�� UI Obj")]
    [SerializeField] GameObject swingBtn;                   //������ư-ȭ����ü
    [SerializeField] GameObject introBox;               //��ġ�� ��ŸƮ
    [SerializeField] GameObject scoreBox;               //���� ������ UI �ڽ�
    [SerializeField] GameObject pitchDataBox;           //���� ������ UI �ڽ�
    [SerializeField] GameObject hitDataBox;             //Ÿ�� ������ UI �ڽ�
    [SerializeField] GameObject gameOverText;           //���� ǥ�� �� ���ӿ��� �ؽ�Ʈ ����
    [SerializeField] GameObject gameOverPanel;          //���ӿ��� ����ȭ��

    int maxCount;
    int bestScore;

    void OnEnable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount += ChangeCount;
            EventManager.Instance.OnSetBallData += GetPitchData;
            EventManager.Instance.OnEnablePitchData += ShowPitchData;
            EventManager.Instance.OnGameReady += InitializeUIAtStart;
            EventManager.Instance.OnScoreChanged += UpdateScore;
            EventManager.Instance.OnHitResult += ShowHitResult;
            EventManager.Instance.OnGameFinished += GameOver;

        }
        else Debug.LogError("HUD �̺�Ʈ ��� ����");
    }
    void Awake()
    {
        CheckNull(introBox, nameof(introBox));
        CheckNull(swingChanceText, nameof(swingChanceText));
        CheckNull(bestScoreText, nameof(bestScoreText));
        CheckNull(currentScoreText, nameof(currentScoreText));
        CheckNull(earnedScoreText, nameof(earnedScoreText));
        CheckNull(finalScoreText, nameof(finalScoreText));
        CheckNull(speedText, nameof(speedText));
        CheckNull(pitchTypeText, nameof(pitchTypeText));
        CheckNull(pitchPosText, nameof(pitchPosText));
        CheckNull(distanceText, nameof(distanceText));
        CheckNull(homerunText, nameof(homerunText));
        CheckNull(timingText, nameof(timingText));
        CheckNull(criticalText, nameof(criticalText));
        CheckNull(bigText, nameof(bigText));
        CheckNull(scoreBox, nameof(scoreBox));
        CheckNull(pitchDataBox, nameof(pitchDataBox));
        CheckNull(hitDataBox, nameof(hitDataBox));
        CheckNull(gameOverText, nameof(gameOverText));
        CheckNull(gameOverPanel, nameof(gameOverPanel));
        CheckNull(swingBtn, nameof(swingBtn));
        swingBtn.GetComponent<Button>().onClick.AddListener(() => EventManager.Instance.PublishOnSwing());
    }
    void CheckNull(Object obj, string name)
    {
        if (obj == null)
            Debug.LogError($"{name} ����");
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
        gameOverPanel.SetActive(false);
        introBox.SetActive(true);
        gameOverText.SetActive(false);
        swingBtn.SetActive(false);
        InitDuringGame();

        powerStat.text="�Ŀ�: "+PlayerManager.Instance.CurrentPower.ToString();
        judgeStat.text = "����: " + PlayerManager.Instance.CurrentJudgeSight.ToString();
        criticalStat.text = "ũ��: " + PlayerManager.Instance.CurrentCritical.ToString();

        
    }
    public void OnIntroTextClicked()
    {
        EventManager.Instance.PublishGameStart();
        introBox.SetActive(false);
        swingBtn.SetActive(true);
    }
    //���� �� �ؽ�Ʈ �ʱ�ȭ
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


    void GetPitchData(BallData ballData)
    {
        speedText.text = $"{ballData.speed:F1}KM";
        pitchPosText.text = ballData.position.ToString();
        pitchTypeText.text = ballData.pitchType.ToString();
    }

    void ShowPitchData(EPitchLocation p)
    {
        pitchDataBox.SetActive(true);
        StartCoroutine(HideHitDataText(pitchDataBox));
    }

    // Ÿ�� ��� ǥ��
    void ShowHitResult(HitResultData hitData)
    {
        if (hitData.isCritical) criticalText.text = "+Critical!!";
        if (hitData.isBigHomerun) bigText.text = "+Big!!!";
        if (hitData.score > 0) earnedScoreText.text = $"+{hitData.score}";

        distanceText.text = $"{hitData.distance}m";
        timingText.text = hitData.timing.ToString();

        // Ȩ�� �˸� ǥ��
        if (hitData.timing == EHitTiming.Miss)
        {
            homerunText.text = "OOPS!!";
        }
        else if (hitData.isHomerun)
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

    void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
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
    IEnumerator GameOverCoroutine()
    {
        gameOverText.SetActive(true);
        yield return StartCoroutine(Fade());
        gameOverText.SetActive(false);
        gameOverPanel.SetActive(true);
        StartCoroutine(ScoreEffect(finalScoreText, 0, ScoreManager.Instance.CurrentScore));
        StartCoroutine(ScoreEffect(earnedCurrencyText, 0, ScoreManager.Instance.RewardCurrency));

    }
    IEnumerator Fade()
    {
        CanvasGroup canvasGroup = gameOverText.GetComponent<CanvasGroup>();
        float timer = 0f;
        while (timer < 2f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timer);
        }
    }
    //���� �ö󰡴� ȿ��
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
    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnSwingCount -= ChangeCount;
            EventManager.Instance.OnSetBallData -= GetPitchData;
            EventManager.Instance.OnEnablePitchData -= ShowPitchData;
            EventManager.Instance.OnGameReady -= InitializeUIAtStart;
            EventManager.Instance.OnScoreChanged -= UpdateScore;
            EventManager.Instance.OnHitResult -= ShowHitResult;
            EventManager.Instance.OnGameFinished -= GameOver;
        }
        else Debug.LogError("HUD �̺�Ʈ ���� ����");
    }
}