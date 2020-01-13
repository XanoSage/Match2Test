using Game;
using System;
using UnityEngine;

namespace User
{
	public class User : IUser
	{
		private const string USER_PROGRESS_KEY = "UserProgress";
		private const string USER_SETTINGS_KEY = "UserSettings";
        public UserSettings Settings => _userSettings;
        private UserSettings _userSettings;

        public int LastCompletedLevel {
            get {
                return _userProgressData.LastLevelCompletedNumber;
            }
        }

        private UserProgressData _userProgressData;

		private User()
		{
            
		}

        public static IUser Create()
		{
			return new User();
		}

		public void AddLevelCompleteData(LevelCompleteData levelCompleteData)
		{
			if (_userProgressData.TryAddLevelCompleteData(levelCompleteData))
				SaveProgressData();
		}

		public LevelCompleteData GetLevelCompleteData(int levelNumber)
		{
			return _userProgressData.GetLevelCompleteData(levelNumber);
		}

		public void Init()
		{
			InitProgressData();
            LoadSettings();
		}

        public void SetSoundActivity(bool active) {
            _userSettings.IsSoundOn = active;
            SaveSettings();
        }

        public void SetMusicActivity(bool active) {
            _userSettings.IsMusicOn = active;
            SaveSettings();
        }

        private void InitProgressData()
		{
			if (PlayerPrefs.HasKey(USER_PROGRESS_KEY))
			{
				var progressJson = PlayerPrefs.GetString(USER_PROGRESS_KEY);
				_userProgressData = JsonUtility.FromJson<UserProgressData>(progressJson);
			}
			else
			{
				_userProgressData = Factory.ProgressFactory.CreateUserProgress();
			}
		}

		private void SaveProgressData()
		{
			var progressJson = JsonUtility.ToJson(_userProgressData, true);
			PlayerPrefs.SetString(USER_PROGRESS_KEY, progressJson);
		}

        private void LoadSettings() {
            if (PlayerPrefs.HasKey(USER_SETTINGS_KEY)) {
                var settingsString = PlayerPrefs.GetString(USER_SETTINGS_KEY);
                _userSettings = JsonUtility.FromJson<UserSettings>(settingsString);
            }
            else {
                _userSettings = Factory.UserFactory.CreateUserSettings(true, true);
                SaveSettings();
            }
        }

        private void SaveSettings() {
            var settingsString = JsonUtility.ToJson(_userSettings, true);
            PlayerPrefs.SetString(USER_SETTINGS_KEY, settingsString);
        }
    }

    
}
