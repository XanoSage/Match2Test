using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsView : UIViewBase
{
    [SerializeField] private Toggle _musicSettings = null;
    [SerializeField] private Toggle _soundSettings = null;

    [SerializeField] private Text _musicText = null;
    [SerializeField] private Text _soundText = null;

    [SerializeField] private Button _okButton = null;

    public event Action<bool> MusicSettingsChangedEvent;
    public event Action<bool> SoundSettingsChangedEvent;
    public event Action OkButtonClickEvent;

    public void Init(bool musicEnabled, bool soundEnabled) {
        _musicSettings.isOn = musicEnabled;
        _soundSettings.isOn = soundEnabled;
        
        SetMusicText(musicEnabled);
        SetSoundText(soundEnabled);
    }

    private void Awake() {
        _musicSettings.onValueChanged.AddListener(OnMusicSettingsValueChanged);
        _soundSettings.onValueChanged.AddListener(OnSoundSettingsValueChanged);
        _okButton.onClick.AddListener(OnOkButtonClickHandler);
    }

    private void OnMusicSettingsValueChanged(bool enabled) {
        MusicSettingsChangedEvent?.Invoke(enabled);
        SetMusicText(enabled);
    }

    private void OnSoundSettingsValueChanged(bool enabled) {
        SoundSettingsChangedEvent?.Invoke(enabled);
        SetSoundText(enabled);
    }

    private void OnOkButtonClickHandler() {
        OkButtonClickEvent?.Invoke();
    }

    private void SetMusicText(bool enabled) {
        var text = enabled ? GameConstants.SettingsData.MUSIC_ON : GameConstants.SettingsData.MUSIC_OFF;
        _musicText.SetText(text);
    }

    private void SetSoundText(bool enabled) {
        var text = enabled ? GameConstants.SettingsData.SOUND_ON : GameConstants.SettingsData.SOUND_OFF;
        _soundText.SetText(text);
    }
}
