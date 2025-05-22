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
        // 버튼 설정
        for (int i = 0; i < buttonMappings.Count; i++)
        {
            ButtonFrameMapping mapping = buttonMappings[i];

            // 텍스트 설정
            mapping.button.GetComponentInChildren<TextMeshProUGUI>().text = ((int)mapping.Frame).ToString() + "FPS";

            // 이벤트 등록
            Frame frame = mapping.Frame;
            Button btn = mapping.button;

            btn.onClick.AddListener(() => SelectedButtonUpdate(btn, frame));
            
            // 현재 프레임 체크
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
        // 중복 선택 무시
        if (currentButton == button) {  return; };

        // 이전 선택 초기화
        if (currentButton != null)
        {
            currentButton.GetComponent<Image>().color = normalColor;
        }

        // 새 선택
        currentButton = button;
        currentFrame = frame;
        currentButton.GetComponent<Image>().color = selectedColor;

        // 적용
        if (apply && GameManager.Instance != null)
        {
            GameManager.Instance.SetFrameRate((int)frame);
        }
    }
}