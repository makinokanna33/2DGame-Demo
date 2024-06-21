using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingWindow : BaseWindow
{
    public Slider BgMusicSldier;
    public Slider SoundSlider;
    public TextMeshProUGUI BgMusicSldierValue;
    public TextMeshProUGUI SoundSliderValue;

    public Button ExitButton;
    private new void Awake()
    {
        base.Awake();
        BgMusicSldier.onValueChanged.AddListener(OnBgMusicVolumeSliderChanged);
        SoundSlider.onValueChanged.AddListener(OnSoundVolumeSliderChanged);

        ExitButton.onClick.AddListener(() =>
        {
            UIManager.CloseWindow(ScreenName.SettingWindow);
        });
    }

    protected override void OnShowIng()
    {
        base.OnShowIng();
        BgMusicSldier.value = PlayerPrefs.GetFloat(MusicManager.BgMusicVolume_SaveKey);
        SoundSlider.value = PlayerPrefs.GetFloat(MusicManager.SoundVolume_SaveKey);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && UIManager.CurrentWindow == ScreenName.SettingWindow)
        {
            UIManager.CloseWindow(ScreenName.SettingWindow);
        }
    }

    private void OnBgMusicVolumeSliderChanged(float value)
    {
        BgMusicSldierValue.text = value.ToString("#0.0");
        MusicManager.Instance.ChangeBgValue(value);
    }

    private void OnSoundVolumeSliderChanged(float value)
    {
        SoundSliderValue.text = value.ToString("#0.0");
        MusicManager.Instance.ChangeSoundValue(value);
    }
}
