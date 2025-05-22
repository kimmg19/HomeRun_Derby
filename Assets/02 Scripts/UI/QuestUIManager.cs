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
        // 패널이 활성화될 때마다 데이터만 업데이트
        if (isInitialized)
        {
            UpdateQuestData();
        }

        // 이벤트 리스너 등록
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnGameFinished += OnGameFinished;
        }
    }

    void OnDisable()
    {
        // 이벤트 리스너 제거
        if (EventManager.Instance != null)
        {
            EventManager.Instance.OnGameFinished -= OnGameFinished;

        }
    }

    // 처음 한 번만 호출 - UI 요소 생성
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

    // 데이터만 업데이트하는 메서드
    void UpdateQuestData()
    {
        List<RepeatableQuestSO> quests = QuestManager.Instance.GetAllQuests();

        for (int i = 0; i < quests.Count && i < questBoxes.Count; i++)
        {
            RepeatableQuestSO quest = quests[i];
            questBoxes[i].UpdateData(quest.CurrentProgress, quest.CurrentTarget);
        }
    }

    // 게임 종료 이벤트 발생 시
    void OnGameFinished()
    {
        // 패널이 활성화된 상태라면 데이터 업데이트
        if (gameObject.activeSelf)
        {
            UpdateQuestData();
        }
    }
    
}