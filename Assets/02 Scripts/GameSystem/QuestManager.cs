using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quest;
using System;

/// <summary>
/// �ݺ� ����Ʈ �ý����� �����ϴ� �Ŵ��� Ŭ����
/// ����Ʈ ���൵ ����, �Ϸ� ó��, ������ ����/�ε� ���
/// </summary>
[DefaultExecutionOrder(-9000)] // EventManager ������ �ʱ�ȭ�ǵ��� ����
public class QuestManager : MonoBehaviour
{
    #region �̱��� �� ����
    // �̱��� �ν��Ͻ�
    static QuestManager instance;
    public static QuestManager Instance { get { return instance; } }

    // ���ӿ��� ��� ������ ��� ����Ʈ ���
    [SerializeField] List<RepeatableQuestSO> quests;

    // ID�� ����Ʈ ��ȸ�� ��ųʸ�
    Dictionary<string, RepeatableQuestSO> questProgress = new();

    // ���� Ű ���λ�
    const string ProgressKey = "Quest_Progress_";
    const string TargetKey = "Quest_Target_";
    #endregion

    #region �����ֱ� �޼���
    void Awake()
    {
        // �̱��� ����
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // ����Ʈ �ʱ�ȭ
        InitializeQuests();
    }

    void OnEnable()
    {
        // ���� ���� �� ����Ʈ ���൵ ������Ʈ
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnGameFinished += ProcessGameResults;
        }
        else Debug.LogError("QuestManager �̺�Ʈ ��� ����");
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnGameFinished -= ProcessGameResults;
        }
    }
    #endregion

    #region ����Ʈ �ʱ�ȭ �� ������ ����
    /// <summary>
    /// ����Ʈ �ʱ�ȭ �� ����� ������ �ε�
    /// </summary>
    void InitializeQuests()
    {
        questProgress.Clear();
        foreach (var quest in quests)
        {
            // ��ȸ�� ��ųʸ� ����
            questProgress[quest.QuestID] = quest;

            // ����� ������ �ε� (ID ���)
            int progress = PlayerPrefs.GetInt(ProgressKey + quest.QuestID, 0);
            int target = PlayerPrefs.GetInt(TargetKey + quest.QuestID, 0);

            // ����Ʈ �ʱ�ȭ
            quest.Initialize(progress, target);
        }
    }

    /// <summary>
    /// ����Ʈ ������ ����
    /// </summary>
    void SaveQuestData()
    {
        foreach (var quest in quests)
        {
            // ID ������� ����
            PlayerPrefs.SetInt(ProgressKey + quest.QuestID, quest.CurrentProgress);
            PlayerPrefs.SetInt(TargetKey + quest.QuestID, quest.CurrentTarget);
        }
        PlayerPrefs.Save();
    }
    #endregion

    #region ���� ��� ó��
    /// <summary>
    /// ���� ���� �� ����Ʈ ���൵ ������Ʈ
    /// </summary>
    void ProcessGameResults()
    {
        // ���� �÷��� ��� ����
        int homerunCount = GetHomerunCount();
        int totalScore = GetTotalScore();
        int totalDistance = GetTotalDistance();
        int perfectHits = GetPerfectHits();

        // ����Ʈ ������ ���൵ ������Ʈ
        UpdateQuestsByType(QuestType.Homerun, homerunCount);
        UpdateQuestsByType(QuestType.TotalScore, totalScore);
        UpdateQuestsByType(QuestType.TotalDistance, totalDistance);
        UpdateQuestsByType(QuestType.PerfectHit, perfectHits);

        // ����� ������ ����
        SaveQuestData();
    }

    /// <summary>
    /// Ư�� ������ ��� ����Ʈ ���൵ ������Ʈ
    /// </summary>
    void UpdateQuestsByType(QuestType type, int value)
    {
        if (value <= 0) return;

        foreach (var quest in quests)
        {
            if (quest.QuestType == type)
            {
                quest.CurrentProgress += value;
            }
        }
    }
    #endregion

    #region ����Ʈ �Ϸ� �� ����
    /// <summary>
    /// ����Ʈ �Ϸ� �� ���� ���� (UI ��ư���� ȣ��)
    /// </summary>
    public void CompleteQuest(string questID)
    {
        if (questProgress.TryGetValue(questID, out RepeatableQuestSO quest))
        {
            if (quest.CurrentState == QuestState.Completed)
            {
                // ���� ����
                GiveReward(quest.QuestReward);

                // ���� ��ǥ ���� (�ʰ��� ����)
                quest.CompleteQuest();

                // ������ ����
                SaveQuestData();
            }
        }
    }

    /// <summary>
    /// ����Ʈ ���� ����
    /// </summary>
    void GiveReward(int currency)
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.AddCurrency(currency);
        }
    }
    #endregion

    #region ���� ��� ������ ����
    int GetHomerunCount()
    {
        // ScoreManager���� Ȩ�� ���� ��������
        return ScoreManager.Instance != null ? ScoreManager.Instance.GetSessionHomerunCount() : 0;
    }

    int GetTotalScore()
    {
        return ScoreManager.Instance != null ? ScoreManager.Instance.CurrentScore : 0;
    }

    int GetTotalDistance()
    {
        // ���� ������ �� ��Ÿ� ���
        return ScoreManager.Instance != null ? (int)ScoreManager.Instance.GetSessionTotalDistance() : 0;
    }

    int GetPerfectHits()
    {
        // ����Ʈ Ÿ�̹� Ÿ�� Ƚ��
        return ScoreManager.Instance != null ? ScoreManager.Instance.GetSessionPerfectHits() : 0;
    }
    #endregion

    #region ����Ʈ ���� ��ȸ
    /// <summary>
    /// UI ǥ�ÿ� ��� ����Ʈ ��� ��ȯ
    /// </summary>
    public List<RepeatableQuestSO> GetAllQuests()
    {
        return quests;
    }

    /// <summary>
    /// ID�� Ư�� ����Ʈ ��ȸ
    /// </summary>
    public RepeatableQuestSO GetQuestByID(string questID)
    {
        questProgress.TryGetValue(questID, out RepeatableQuestSO quest);
        return quest;
    }
    #endregion
}