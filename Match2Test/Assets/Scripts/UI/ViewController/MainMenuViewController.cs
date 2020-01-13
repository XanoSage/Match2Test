using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class MainMenuViewController : IMainMenuViewController
    {
        private readonly MainMenuView _mainMenuView;

        public event Action PlayButtonClickEvent;
        public event Action SettingsButtonClickEvent;

        private MainMenuViewController(MainMenuView mainMenuView) {
            _mainMenuView = mainMenuView;
            _mainMenuView.PlayButtonClickEvent += OnPlayButtonClickHandler;
            _mainMenuView.SettingsButtonClickEvent += OnSettingsButtonClickHandler;
        }

        public static IMainMenuViewController Create(MainMenuView mainMenuView) {
            return new MainMenuViewController(mainMenuView);
        }

        public void Hide() {
            _mainMenuView.Hide();
        }

        public void Show() {
            _mainMenuView.Show();
        }

        private void OnPlayButtonClickHandler() {
            PlayButtonClickEvent?.Invoke();
        }

        private void OnSettingsButtonClickHandler() {
            SettingsButtonClickEvent?.Invoke();
        }
    }

    public interface IMainMenuViewController:IBaseUIController
    {
        event Action PlayButtonClickEvent;
        event Action SettingsButtonClickEvent;
    }
}