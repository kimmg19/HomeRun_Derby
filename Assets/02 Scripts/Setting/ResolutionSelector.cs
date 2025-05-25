using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ResolutionScale
{
    Low = 50,     // 50%
    Medium = 75,  // 75% 
    High = 100    // 100%
}

public class ResolutionSelector : MonoBehaviour
{
    [System.Serializable]
    public struct ButtonResolutionMapping
    {
        public Button button;
        public ResolutionScale resolutionScale;
    }

    [SerializeField] List<ButtonResolutionMapping> buttonMappings;
    [SerializeField] Color selectedColor;
    [SerializeField] Color normalColor;

    ResolutionScale currentResolution;
    Button currentButton;

    int originalWidth;
    int originalHeight;

    void Start()
    {
        originalWidth = Screen.width;
        originalHeight = Screen.height;

        for (int i = 0; i < buttonMappings.Count; i++)
        {
            ButtonResolutionMapping mapping = buttonMappings[i];

            mapping.button.GetComponentInChildren<TextMeshProUGUI>().text = ((int)mapping.resolutionScale).ToString() + "%";

            ResolutionScale scale = mapping.resolutionScale;
            Button btn = mapping.button;

            btn.onClick.AddListener(() => SelectedButtonUpdate(btn, scale));

            if (mapping.resolutionScale == ResolutionScale.High)
            {
                SelectedButtonUpdate(btn, scale, false);
            }
        }
    }

    void SelectedButtonUpdate(Button button, ResolutionScale scale, bool apply = true)
    {
        if (currentButton == button) return;

        if (currentButton != null)
        {
            currentButton.GetComponent<Image>().color = normalColor;
        }

        currentButton = button;
        currentResolution = scale;
        currentButton.GetComponent<Image>().color = selectedColor;

        if (apply)
        {
            ApplyResolution(scale);
        }
    }

    void ApplyResolution(ResolutionScale scale)
    {
        float scaleFactor = (float)scale / 100f;

        int targetWidth = Mathf.RoundToInt(originalWidth * scaleFactor);
        int targetHeight = Mathf.RoundToInt(originalHeight * scaleFactor);

        Screen.SetResolution(targetWidth, targetHeight, true);
    }
}