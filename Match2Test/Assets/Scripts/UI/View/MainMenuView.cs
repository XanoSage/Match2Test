using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuView : UIViewBase
{
    [SerializeField] private Button _playButton = null;
    [SerializeField] private Button _settingsButton = null;

    public event Action PlayButtonClickEvent;
    public event Action SettingsButtonClickEvent;

    private void Awake() {
        _playButton.onClick.AddListener(OnPlayButtonClickHandler);
        _settingsButton.onClick.AddListener(OnSettingsButtonClickHandler);
    }

    private void OnPlayButtonClickHandler() {
        PlayButtonClickEvent?.Invoke();
    }

    private void OnSettingsButtonClickHandler() {
        SettingsButtonClickEvent?.Invoke();
    }
}
