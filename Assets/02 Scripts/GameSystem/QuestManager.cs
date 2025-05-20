using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quest;
using System;

/// <summary>
/// 반복 퀘스트 시스템을 관리하는 매니저 클래스
/// 퀘스트 진행도 추적, 완료 처리, 데이터 저장/로드 담당
/// </summary>
[DefaultExecutionOrder(-9000)] // EventManager 다음에 초기화되도록 설정
public class QuestManager : MonoBehaviour
{
    #region 싱글톤 및 변수
    // 싱글톤 인스턴스
    static QuestManager instance;
    public static QuestManager Instance { get { return instance; } }

    // 게임에서 사용 가능한 모든 퀘스트 목록
    [SerializeField] List<RepeatableQuestSO> quests;

    // ID별 퀘스트 조회용 딕셔너리
    Dictionary<string, RepeatableQuestSO> questProgress = new();

    // 저장 키 접두사
    const string ProgressKey = "Quest_Progress_";
    const string TargetKey = "Quest_Target_";
    #endregion

    #region 생명주기 메서드
    void Awake()
    {
        // 싱글톤 패턴
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);

        // 퀘스트 초기화
        InitializeQuests();
    }

    void OnEnable()
    {
        // 게임 종료 시 퀘스트 진행도 업데이트
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnGameFinished += ProcessGameResults;
        }
        else Debug.LogError("QuestManager 이벤트 등록 실패");
    }

    void OnDisable()
    {
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnGameFinished -= ProcessGameResults;
        }
    }
    #endregion

    #region 퀘스트 초기화 및 데이터 관리
    /// <summary>
    /// 퀘스트 초기화 및 저장된 데이터 로드
    /// </summary>
    void InitializeQuests()
    {
        questProgress.Clear();
        foreach (var quest in quests)
        {
            // 조회용 딕셔너리 구성
            questProgress[quest.QuestID] = quest;

            // 저장된 데이터 로드 (ID 기반)
            int progress = PlayerPrefs.GetInt(ProgressKey + quest.QuestID, 0);
            int target = PlayerPrefs.GetInt(TargetKey + quest.QuestID, 0);

            // 퀘스트 초기화
            quest.Initialize(progress, target);
        }
    }

    /// <summary>
    /// 퀘스트 데이터 저장
    /// </summary>
    void SaveQuestData()
    {
        foreach (var quest in quests)
        {
            // ID 기반으로 저장
            PlayerPrefs.SetInt(ProgressKey + quest.QuestID, quest.CurrentProgress);
            PlayerPrefs.SetInt(TargetKey + quest.QuestID, quest.CurrentTarget);
        }
        PlayerPrefs.Save();
    }
    #endregion

    #region 게임 결과 처리
    /// <summary>
    /// 게임 종료 시 퀘스트 진행도 업데이트
    /// </summary>
    void ProcessGameResults()
    {
        // 게임 플레이 결과 수집
        int homerunCount = GetHomerunCount();
        int totalScore = GetTotalScore();
        int totalDistance = GetTotalDistance();
        int perfectHits = GetPerfectHits();

        // 퀘스트 유형별 진행도 업데이트
        UpdateQuestsByType(QuestType.Homerun, homerunCount);
        UpdateQuestsByType(QuestType.TotalScore, totalScore);
        UpdateQuestsByType(QuestType.TotalDistance, totalDistance);
        UpdateQuestsByType(QuestType.PerfectHit, perfectHits);

        // 변경된 데이터 저장
        SaveQuestData();
    }

    /// <summary>
    /// 특정 유형의 모든 퀘스트 진행도 업데이트
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

    #region 퀘스트 완료 및 보상
    /// <summary>
    /// 퀘스트 완료 및 보상 지급 (UI 버튼에서 호출)
    /// </summary>
    public void CompleteQuest(string questID)
    {
        if (questProgress.TryGetValue(questID, out RepeatableQuestSO quest))
        {
            if (quest.CurrentState == QuestState.Completed)
            {
                // 보상 지급
                GiveReward(quest.QuestReward);

                // 다음 목표 설정 (초과분 유지)
                quest.CompleteQuest();

                // 데이터 저장
                SaveQuestData();
            }
        }
    }

    /// <summary>
    /// 퀘스트 보상 지급
    /// </summary>
    void GiveReward(int currency)
    {
        if (PlayerManager.Instance != null)
        {
            PlayerManager.Instance.AddCurrency(currency);
        }
    }
    #endregion

    #region 게임 통계 데이터 수집
    int GetHomerunCount()
    {
        // ScoreManager에서 홈런 개수 가져오기
        return ScoreManager.Instance != null ? ScoreManager.Instance.GetSessionHomerunCount() : 0;
    }

    int GetTotalScore()
    {
        return ScoreManager.Instance != null ? ScoreManager.Instance.CurrentScore : 0;
    }

    int GetTotalDistance()
    {
        // 게임 세션의 총 비거리 계산
        return ScoreManager.Instance != null ? (int)ScoreManager.Instance.GetSessionTotalDistance() : 0;
    }

    int GetPerfectHits()
    {
        // 퍼펙트 타이밍 타격 횟수
        return ScoreManager.Instance != null ? ScoreManager.Instance.GetSessionPerfectHits() : 0;
    }
    #endregion

    #region 퀘스트 정보 조회
    /// <summary>
    /// UI 표시용 모든 퀘스트 목록 반환
    /// </summary>
    public List<RepeatableQuestSO> GetAllQuests()
    {
        return quests;
    }

    /// <summary>
    /// ID로 특정 퀘스트 조회
    /// </summary>
    public RepeatableQuestSO GetQuestByID(string questID)
    {
        questProgress.TryGetValue(questID, out RepeatableQuestSO quest);
        return quest;
    }
    #endregion
}