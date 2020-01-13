using System;

namespace User
{
	public interface IUser
	{
		void Init();
		int LastCompletedLevel { get; }
        UserSettings Settings { get; }
		LevelCompleteData GetLevelCompleteData(int levelNumber);
		void AddLevelCompleteData(LevelCompleteData levelCompleteData);
        void SetSoundActivity(bool active);
        void SetMusicActivity(bool active);
	}

    [Serializable]
    public struct UserSettings {
        public bool IsMusicOn;
        public bool IsSoundOn;

        public UserSettings(bool isMusicOn, bool isSoundOn) {
            IsMusicOn = isMusicOn;
            IsSoundOn = isSoundOn;
        }
    }
}
