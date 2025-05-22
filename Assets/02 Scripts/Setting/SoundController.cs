using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SoundController : MonoBehaviour
{
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider bgmSlider;
    [SerializeField] AudioMixer mixer;

    // PlayerPrefs 키
    const string SFX_KEY = "SFXVolume";
    const string BGM_KEY = "BGMVolume";

    void OnEnable()
    {
        // 저장된 값 불러오기
        LoadVolumes();

        // 이벤트 등록
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        bgmSlider.onValueChanged.AddListener(SetBGMVolume);
    }

    // 볼륨 설정 불러오기
    void LoadVolumes()
    {
        // 저장된 값이 없으면 기본값 1 사용
        float sfxVolume = PlayerPrefs.GetFloat(SFX_KEY, 1f);
        float bgmVolume = PlayerPrefs.GetFloat(BGM_KEY, 1f);

        // 슬라이더에 값 적용 (이벤트 트리거됨)
        sfxSlider.value = sfxVolume;
        bgmSlider.value = bgmVolume;
    }

    public void SetSFXVolume(float value)
    {
        // 슬라이더 최소값이 이미 0.001 이상으로 설정되어 있으므로 별도 체크 불필요
        mixer.SetFloat("SFX", Mathf.Log10(value) * 20);

        // 설정 저장
        PlayerPrefs.SetFloat(SFX_KEY, value);
        PlayerPrefs.Save();
    }

    public void SetBGMVolume(float value)
    {
        mixer.SetFloat("BGM", Mathf.Log10(value) * 20);

        // 설정 저장
        PlayerPrefs.SetFloat(BGM_KEY, value);
        PlayerPrefs.Save();
    }

    void OnDisable()
    {
        sfxSlider.onValueChanged.RemoveAllListeners();
        bgmSlider.onValueChanged.RemoveAllListeners();
    }
}