using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Quest;

/// <summary>
/// ����Ʈ�� �⺻ ������ ��� ����ȭ ������ Ŭ����
/// </summary>
[System.Serializable]
public class QuestInfo
{
    public string questID;      // ����Ʈ�� �����ϰ� �ĺ��ϴ� ID
    public string questName;    // UI�� ǥ�õ� ����Ʈ �̸�
    public int questReward;     // ����Ʈ �Ϸ� �� ���޵Ǵ� ��ȭ ����
    public int baseTargetValue;     // ����Ʈ �Ϸῡ �ʿ��� ��ǥ ��ġ (��: Ȩ�� 10��)
}

/// <summary>
/// �ݺ� ���� ������ ����Ʈ�� �����ϴ� ScriptableObject
/// ����Ʈ ������ ���� �����÷��� ����� �ٸ��� �ݿ�
/// </summary>
[CreateAssetMenu(fileName = "Quest", menuName = "HomerunDerby/Quest")]
public class RepeatableQuestSO : ScriptableObject
{
    [SerializeField] QuestInfo quest;
    [SerializeField] QuestType questType;
    [SerializeField] int targetValueIncrease = 10;  //����Ʈ�� �ٸ� ��ġ ����.

    [System.NonSerialized] int currentProgress;  // ���� ���൵
    [System.NonSerialized] int currentTarget;    // ���� ��ǥ�� (������ ���� ����)

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
    /// ���� ��ǥ�� ������
    /// </summary>
    public int CurrentTarget
    {
        get => currentTarget > 0 ? currentTarget : quest.baseTargetValue;
        set => currentTarget = value;
    }

   
    /// <summary>
    /// ����Ʈ�� ���� ����(���� ��/�Ϸ�) ���
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
        // ����Ʈ�� �Ϸ� ���°� �ƴϸ� 0 ��ȯ
        if (CurrentState != QuestState.Completed)
            return 0;

        // ���� ��ǥġ ����
        int previousTarget = currentTarget;

        // ��ǥ�� ����
        currentTarget += targetValueIncrease;

        // �ʰ��� ��� (���� ���൵ - ���� ��ǥġ)
        int overflow = currentProgress - previousTarget;

        // �ʰ��� ��ȯ (���� ��ǥ���� �󸶳� ����Ǿ����� ǥ�� ����)
        return overflow;
    }
    /// <summary>
    /// ����Ʈ �ʱ�ȭ (����� ������ ����)
    /// </summary>
    public void Initialize(int progress, int target)
    {
        currentProgress = progress;
        currentTarget = target > 0 ? target : quest.baseTargetValue;
    }
}
