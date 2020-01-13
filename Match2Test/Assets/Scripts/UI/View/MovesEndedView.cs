using System;
using UnityEngine;
using UnityEngine.UI;

public class MovesEndedView : UIViewBase
{
    [SerializeField] private Button _mainMenuButton = null;
    [SerializeField] private Button _restartMenuButton = null;
    [SerializeField] private Text _titleText = null;

    public event Action MainMenuButtonClickEvent;
    public event Action RestartButtonClickEvent;

    public void Init(string title) {
        _titleText.text = title;
    }

    private void Awake() {
        _mainMenuButton.onClick.AddListener(OnMainMenuButtonClickHandler);
        _restartMenuButton.onClick.AddListener(OnRestartButtonClickHandler);
    }

    private void OnMainMenuButtonClickHandler() {
        MainMenuButtonClickEvent?.Invoke();
    }

    private void OnRestartButtonClickHandler() {
        RestartButtonClickEvent?.Invoke();
    }
}
