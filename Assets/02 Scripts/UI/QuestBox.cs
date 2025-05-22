using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quest;

public class QuestBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI questNameText;
    [SerializeField] Slider progressBar;
    [SerializeField] TextMeshProUGUI rewardText;
    [SerializeField] TextMeshProUGUI valueText;
    [SerializeField] Button completeButton;  // 퀘스트 완료 버튼

    // QuestID를 저장
    public string QuestID { get; private set; }

    // 초기화 메서드 - 첫 설정
    public void Init(string id, string name, int reward, int current, int target)
    {
        QuestID = id;
        questNameText.text = name;
        rewardText.text = reward.ToString();
        UpdateData(current, target);

        // 완료 버튼 이벤트 설정
        if (completeButton != null)
        {
            completeButton.onClick.RemoveAllListeners();
            completeButton.onClick.AddListener(OnCompleteQuest);

            // 초기 버튼 상태 설정
            UpdateButtonState(current >= target);
        }
    }

    // 데이터만 업데이트하는 메서드
    public void UpdateData(int current, int target)
    {
        valueText.text = current.ToString() + " / " + target.ToString();
        progressBar.value = target > 0 ? (float)current / target : 0;

        // 버튼 상태도 업데이트
        if (completeButton != null)
        {
            UpdateButtonState(current >= target);
        }
    }

    // 버튼 상태 업데이트
    void UpdateButtonState(bool isCompleted)
    {
        completeButton.interactable = isCompleted;
    }

    // 퀘스트 완료 버튼 클릭 이벤트
    void OnCompleteQuest()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.CompleteQuest(QuestID);

            // 완료 후 데이터 업데이트
            RepeatableQuestSO quest = QuestManager.Instance.GetQuestByID(QuestID);
            if (quest != null)
            {
                UpdateData(quest.CurrentProgress, quest.CurrentTarget);
            }
        }
    }
}