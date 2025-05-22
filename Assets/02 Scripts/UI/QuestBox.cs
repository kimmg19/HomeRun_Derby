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
    [SerializeField] Button completeButton;  // ����Ʈ �Ϸ� ��ư

    // QuestID�� ����
    public string QuestID { get; private set; }

    // �ʱ�ȭ �޼��� - ù ����
    public void Init(string id, string name, int reward, int current, int target)
    {
        QuestID = id;
        questNameText.text = name;
        rewardText.text = reward.ToString();
        UpdateData(current, target);

        // �Ϸ� ��ư �̺�Ʈ ����
        if (completeButton != null)
        {
            completeButton.onClick.RemoveAllListeners();
            completeButton.onClick.AddListener(OnCompleteQuest);

            // �ʱ� ��ư ���� ����
            UpdateButtonState(current >= target);
        }
    }

    // �����͸� ������Ʈ�ϴ� �޼���
    public void UpdateData(int current, int target)
    {
        valueText.text = current.ToString() + " / " + target.ToString();
        progressBar.value = target > 0 ? (float)current / target : 0;

        // ��ư ���µ� ������Ʈ
        if (completeButton != null)
        {
            UpdateButtonState(current >= target);
        }
    }

    // ��ư ���� ������Ʈ
    void UpdateButtonState(bool isCompleted)
    {
        completeButton.interactable = isCompleted;
    }

    // ����Ʈ �Ϸ� ��ư Ŭ�� �̺�Ʈ
    void OnCompleteQuest()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.CompleteQuest(QuestID);

            // �Ϸ� �� ������ ������Ʈ
            RepeatableQuestSO quest = QuestManager.Instance.GetQuestByID(QuestID);
            if (quest != null)
            {
                UpdateData(quest.CurrentProgress, quest.CurrentTarget);
            }
        }
    }
}