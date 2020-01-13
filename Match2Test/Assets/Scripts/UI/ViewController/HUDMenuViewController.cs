using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game.UI {
    public class HUDMenuViewController : IHUDMenuViewController {
        public event Action MainMenuButtonClickEvent;
        public event Action SettingsMenuButtonClickEvent;
        public event Action RestartButtonClickEvent;
        public event Action CloseButtonClickEvent;

        private readonly HUDMenuView _hudMenuView;

        private HUDMenuViewController(HUDMenuView hudMenuView) {
            _hudMenuView = hudMenuView;
            
            _hudMenuView.MainMenuButtonClickEvent += OnMainMenuButtonClickHandler;
            _hudMenuView.SettingsMenuButtonClickEvent += OnSettingsMenuButtonClickHandler;
            _hudMenuView.RestartButtonClickEvent += OnRestartButtonClickHandler;
            _hudMenuView.CloseButtonClickEvent += OnCloseButtonClickHandler;

            Hide();
        }

        public static IHUDMenuViewController Create(HUDMenuView hudMenuView) {
            return new HUDMenuViewController(hudMenuView);
        }

        public void Hide() {
            _hudMenuView.Hide();
        }

        public void Show() {
            _hudMenuView.Show();
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

    public interface IHUDMenuViewController: IBaseUIController {
        event Action MainMenuButtonClickEvent;
        event Action SettingsMenuButtonClickEvent;
        event Action RestartButtonClickEvent;
        event Action CloseButtonClickEvent;
    }
}