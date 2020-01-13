using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class SettingsViewController : ISettingsViewController
    {
        private SettingsView _settingsView;

        public event Action<bool> MusicSettingsChangedEvent;
        public event Action<bool> SoundSettingsChangedEvent;
        public event Action OkButtonClickEvent;

        private SettingsViewController(SettingsView settingsView) {
            _settingsView = settingsView;
            _settingsView.MusicSettingsChangedEvent += OnMusicSettingsChangedHandler;
            _settingsView.SoundSettingsChangedEvent += OnSoundSettingsChangedHandler;
            _settingsView.OkButtonClickEvent += OnOkButtonClickHandler;
            Hide();
        }

        public static ISettingsViewController Create(SettingsView settingsView) {
            return new SettingsViewController(settingsView);
        }

        public void Hide() {
            _settingsView.Hide();
        }

        public void Init(bool musicEnabled, bool soundEnabled) {
            _settingsView.Init(musicEnabled, soundEnabled);
        }

        public void Show() {
            _settingsView.Show();
        }

        private void OnMusicSettingsChangedHandler(bool enabled) {
            MusicSettingsChangedEvent?.Invoke(enabled);
        }

        private void OnSoundSettingsChangedHandler(bool enabled) {
            SoundSettingsChangedEvent?.Invoke(enabled);
        }

        private void OnOkButtonClickHandler() {
            OkButtonClickEvent?.Invoke();
        }
    }

    public interface ISettingsViewController:IBaseUIController
    {
        void Init(bool musicEnabled, bool soundEnabled);
        event Action<bool> MusicSettingsChangedEvent;
        event Action<bool> SoundSettingsChangedEvent;
        event Action OkButtonClickEvent;
    }
}