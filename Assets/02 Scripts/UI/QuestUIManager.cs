using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestUIManager : MonoBehaviour
{
    [SerializeField] GameObject questBoxPrefab;
    [SerializeField] Transform contentBox;
    List<QuestBox> questBoxes = new();

    bool isInitialized = false;

    void Start()
    {
        if (!isInitialized)
        {
            InitQuestUI();
            isInitialized = true;
        }
    }

    void OnEnable()
    {
        // �г��� Ȱ��ȭ�� ������ �����͸� ������Ʈ
        if (isInitialized)
        {
            UpdateQuestData();
        }

        // �̺�Ʈ ������ ���
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnGameFinished += OnGameFinished;
        }
    }

    void OnDisable()
    {
        // �̺�Ʈ ������ ����
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnGameFinished -= OnGameFinished;

        }
    }

    // ó�� �� ���� ȣ�� - UI ��� ����
    void InitQuestUI()
    {
        List<RepeatableQuestSO> quests = QuestManager.Instance.GetAllQuests();

        for (int i = 0; i < quests.Count; i++)
        {
            GameObject questBoxObj = Instantiate(questBoxPrefab, contentBox);
            QuestBox box = questBoxObj.GetComponent<QuestBox>();
            questBoxes.Add(box);

            RepeatableQuestSO quest = quests[i];
            box.Init(quest.QuestID, quest.QuestName, quest.QuestReward,
                     quest.CurrentProgress, quest.CurrentTarget);
        }
    }

    // �����͸� ������Ʈ�ϴ� �޼���
    void UpdateQuestData()
    {
        List<RepeatableQuestSO> quests = QuestManager.Instance.GetAllQuests();

        for (int i = 0; i < quests.Count && i < questBoxes.Count; i++)
        {
            RepeatableQuestSO quest = quests[i];
            questBoxes[i].UpdateData(quest.CurrentProgress, quest.CurrentTarget);
        }
    }

    // ���� ���� �̺�Ʈ �߻� ��
    void OnGameFinished()
    {
        // �г��� Ȱ��ȭ�� ���¶�� ������ ������Ʈ
        if (gameObject.activeSelf)
        {
            UpdateQuestData();
        }
    }
    
}