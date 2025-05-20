using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quest;

/// <summary>
/// 퀘스트의 기본 정보를 담는 직렬화 가능한 클래스
/// </summary>
[System.Serializable]
public class QuestInfo
{
    public string questID;      // 퀘스트를 고유하게 식별하는 ID
    public string questName;    // UI에 표시될 퀘스트 이름
    public int questReward;     // 퀘스트 완료 시 지급되는 통화 보상
    public int baseTargetValue;     // 퀘스트 완료에 필요한 목표 수치 (예: 홈런 10개)
}

/// <summary>
/// 반복 수행 가능한 퀘스트를 정의하는 ScriptableObject
/// 퀘스트 유형에 따라 게임플레이 결과를 다르게 반영
/// </summary>
[CreateAssetMenu(fileName = "Quest", menuName = "HomerunDerby/Quest")]
public class RepeatableQuestSO : ScriptableObject
{
    [SerializeField] QuestInfo quest;
    [SerializeField] QuestType questType;
    [SerializeField] int targetValueIncrease = 10;  //퀘스트별 다른 수치 적용.

    [System.NonSerialized] int currentProgress;  // 현재 진행도
    [System.NonSerialized] int currentTarget;    // 현재 목표값 (레벨에 따라 증가)

    public string QuestID=>quest.questID;
    public string QuestName=>quest.questName;
    public int QuestReward => quest.questReward;
    public int BaseTargetValue=>quest.baseTargetValue;
    public QuestType QuestType=>questType;

    public int CurrentProgress
    {
        get =>currentProgress; 
        set =>currentProgress = value; 
    }
    /// <summary>
    /// 현재 목표값 접근자
    /// </summary>
    public int CurrentTarget
    {
        get => currentTarget > 0 ? currentTarget : quest.baseTargetValue;
        set => currentTarget = value;
    }

   
    /// <summary>
    /// 퀘스트의 현재 상태(진행 중/완료) 계산
    /// </summary>
    public QuestState CurrentState=>
        currentProgress>=CurrentTarget ? QuestState.Completed : QuestState.InProgress;

    public string GetProgressTest()
    {
        return $"{CurrentProgress}/{CurrentTarget}";
    }

    public float GetProgressPercentage()
    {
        return (float)CurrentProgress/CurrentTarget;
    }

    public int CompleteQuest()
    {
        // 퀘스트가 완료 상태가 아니면 0 반환
        if (CurrentState != QuestState.Completed)
            return 0;

        // 현재 목표치 저장
        int previousTarget = currentTarget;

        // 목표값 증가
        currentTarget += targetValueIncrease;

        // 초과분 계산 (현재 진행도 - 이전 목표치)
        int overflow = currentProgress - previousTarget;

        // 초과분 반환 (다음 목표까지 얼마나 진행되었는지 표시 가능)
        return overflow;
    }
    /// <summary>
    /// 퀘스트 초기화 (저장된 값으로 설정)
    /// </summary>
    public void Initialize(int progress, int target)
    {
        currentProgress = progress;
        currentTarget = target > 0 ? target : quest.baseTargetValue;
    }
}
