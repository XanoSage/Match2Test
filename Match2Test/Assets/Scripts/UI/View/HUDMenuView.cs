using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDMenuView : UIViewBase {
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _settingsMenuButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _closeButton;

    public event Action MainMenuButtonClickEvent;
    public event Action SettingsMenuButtonClickEvent;
    public event Action RestartButtonClickEvent;
    public event Action CloseButtonClickEvent;

    
    private void Start()
    {
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClickHandler);
        _settingsMenuButton.onClick.AddListener(OnSettingsMenuButtonClickHandler);
        _restartButton.onClick.AddListener(OnRestartButtonClickHandler);
        _closeButton.onClick.AddListener(OnCloseButtonClickHandler);
    }

    private void OnMainMenuButtonClickHandler() {
        MainMenuButtonClickEvent?.Invoke();
    }

    private void OnSettingsMenuButtonClickHandler() {
        SettingsMenuButtonClickEvent?.Invoke();
    }

    private void OnRestartButtonClickHandler() {
        RestartButtonClickEvent?.Invoke();
    }

    private void OnCloseButtonClickHandler() {
        CloseButtonClickEvent?.Invoke();
    }
}
