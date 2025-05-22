using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum Frame
{
    Low = 30,
    Middle = 60,
    High = 120
}

public class FrameSelector : MonoBehaviour
{
    [System.Serializable]
    public struct ButtonFrameMapping
    {
        public Button button;
        public Frame Frame;
    }

    [SerializeField] List<ButtonFrameMapping> buttonMappings;
    [SerializeField] Color selectedColor;
    [SerializeField] Color normalColor;

    Frame currentFrame;
    Button currentButton;

    void Start()
    {
        // ��ư ����
        for (int i = 0; i < buttonMappings.Count; i++)
        {
            ButtonFrameMapping mapping = buttonMappings[i];

            // �ؽ�Ʈ ����
            mapping.button.GetComponentInChildren<TextMeshProUGUI>().text = ((int)mapping.Frame).ToString() + "FPS";

            // �̺�Ʈ ���
            Frame frame = mapping.Frame;
            Button btn = mapping.button;

            btn.onClick.AddListener(() => SelectedButtonUpdate(btn, frame));
            
            // ���� ������ üũ
            if (GameManager.Instance != null &&
                (int)mapping.Frame == GameManager.Instance.CurrentFrameRate)
            {
                SelectedButtonUpdate(btn, frame, false);
            }
        }
    }

    void OnEnable()
    {       

        if (GameManager.Instance != null)
        {
            SyncSelection();
        }
    }

    void SyncSelection()
    {
        int current = GameManager.Instance.CurrentFrameRate;

        for (int i = 0; i < buttonMappings.Count; i++)
        {
            if ((int)buttonMappings[i].Frame == current)
            {
                SelectedButtonUpdate(buttonMappings[i].button, buttonMappings[i].Frame, false);
                break;
            }
        }
    }

    void SelectedButtonUpdate(Button button, Frame frame, bool apply = true)
    {
        // �ߺ� ���� ����
        if (currentButton == button) {  return; };

        // ���� ���� �ʱ�ȭ
        if (currentButton != null)
        {
            currentButton.GetComponent<Image>().color = normalColor;
        }

        // �� ����
        currentButton = button;
        currentFrame = frame;
        currentButton.GetComponent<Image>().color = selectedColor;

        // ����
        if (apply && GameManager.Instance != null)
        {
            GameManager.Instance.SetFrameRate((int)frame);
        }
    }
}