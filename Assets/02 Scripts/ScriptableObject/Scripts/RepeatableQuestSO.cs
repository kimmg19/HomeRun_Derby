using Quest;
using UnityEngine;

[System.Serializable]
public class QuestInfo
{
    public string questID;          // ID
    public string questName;        // �̸�
    public int questReward;         // ����
    public int baseTargetValue;     // �⺻ ��ǥ
}

[CreateAssetMenu(fileName = "Quest", menuName = "HomerunDerby/Quest")]
public class RepeatableQuestSO : ScriptableObject
{
    [SerializeField] QuestInfo quest;
    [SerializeField] QuestType questType;
    [SerializeField] int targetValueIncrease = 10;  // ������

    [System.NonSerialized] int currentProgress;  // ���൵
    [System.NonSerialized] int currentTarget;    // ��ǥ��
    public string QuestID => quest.questID;
    public string QuestName => quest.questName;
    public int QuestReward => quest.questReward;
    public int BaseTargetValue => quest.baseTargetValue;
    public QuestType QuestType => questType;

    public int CurrentProgress
    {
        get => currentProgress;
        set => currentProgress = value;
    }

    public int CurrentTarget
    {
        get => currentTarget > 0 ? currentTarget : BaseTargetValue;
        set => currentTarget = value;
    }

    public QuestState CurrentState =>
        currentProgress >= CurrentTarget ? QuestState.Completed : QuestState.InProgress;       

    public int CompleteQuest()
    {
        if (CurrentState != QuestState.Completed)
            return 0;

        int previousTarget = currentTarget;
        currentTarget += targetValueIncrease;
        int overflow = currentProgress - previousTarget;
        return overflow;
    }

    public void Initialize(int progress, int target)
    {
        currentProgress = progress;
        currentTarget = target > 0 ? target : quest.baseTargetValue;
    }
}