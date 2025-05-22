using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] AudioMixer mixer;

    // PlayerPrefs Ű
    const string SFX_KEY = "SFXVolume";
    const string BGM_KEY = "BGMVolume";

    void OnEnable()
    {
        // ����� �� �ҷ�����
        LoadVolumes();

        // �̺�Ʈ ���
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    // ���� ���� �ҷ�����
    void LoadVolumes()
    {
        // ����� ���� ������ �⺻�� 1 ���
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        float bgmVolume = PlayerPrefs.GetFloat(BGM_KEY, 1f);

        // �����̴��� �� ���� (�̺�Ʈ Ʈ���ŵ�)
        sfxSlider.value = sfxVolume;
        bgmSlider.value = bgmVolume;
    }

    public void SetSFXVolume(float value)
    {
        // �����̴� �ּҰ��� �̹� 0.001 �̻����� �����Ǿ� �����Ƿ� ���� üũ ���ʿ�
        mixer.SetFloat("SFX", Mathf.Log10(value) * 20);

        // ���� ����
        PlayerPrefs.SetFloat(SFX_KEY, value);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume(float value)
    {
        mixer.SetFloat("BGM", Mathf.Log10(value) * 20);

        // ���� ����
        PlayerPrefs.SetFloat(BGM_KEY, value);
        PlayerPrefs.Save();
    }

    void OnDisable()
    {
        sfxSlider.onValueChanged.RemoveAllListeners();
        bgmSlider.onValueChanged.RemoveAllListeners();
    }
}