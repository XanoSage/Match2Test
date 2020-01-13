using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageBoxView : UIViewBase
{
    [SerializeField] private Text _titleText = null;
    [SerializeField] private Text _description = null;
    [SerializeField] private Button _firstButton = null;
    [SerializeField] private Button _secondButton = null;
    [SerializeField] private Text _firstButtonText = null;
    [SerializeField] private Text _secondButtonText = null;
    
    private Action _firstButtonAction;
    private Action _secondButtonAction;
    
    private void Awake() {
        _firstButton.onClick.AddListener(OnFirstButtonClickHandler);
        _secondButton.onClick.AddListener(OnSecondButtonClickHandler);        
    }

    public void Init(
        string title, 
        string description, 
        string firstButtonText, 
        Action onFirstButtonAction) {
        _titleText.SetText(title);
        _description.SetText(description);
        _firstButtonText.SetText(firstButtonText);
        _firstButtonAction = onFirstButtonAction;
        HideButton(_secondButton);
    }

    public void Init(
        string title, 
        string description, 
        string firstButtonText, 
        string secondButtonText, 
        Action onFirstButtonAction, 
        Action onSecondButtonAction) {
        _titleText.SetText(title);
        _description.SetText(description);
        _firstButtonText.SetText(firstButtonText);
        _firstButtonAction = onFirstButtonAction;
        _secondButtonText.SetText(secondButtonText);
        _secondButtonAction = onSecondButtonAction;
        ShowButton(_secondButton);
    }

    private void OnFirstButtonClickHandler() {
        _firstButtonAction?.Invoke();
    }

    private void OnSecondButtonClickHandler() {
        _secondButtonAction?.Invoke();
    }

    private void ShowButton(Button button) {
        button.gameObject.SetActive(true);
    }

    private void HideButton(Button button) {
        button.gameObject.SetActive(false);
    }
}

public static class UIExtension
{
    public static void SetText(this Text textView, string text) {
        textView.text = text;
    }
}

