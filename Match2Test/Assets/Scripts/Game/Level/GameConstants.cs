namespace Game
{
	public static class GameConstants
	{
		public static class GameData
		{
			public const float DELAY_BEFORE_DROP_DOWN = 0.25f;
			public const float ANIMATION_DURATION = 0.3f;
			public const int STACK_ITERATION_COUNT = 1000;
		}

		public static class BonusData
		{
			public const float BONUS_CREATION_TIME = 0.5f;
			public const float ROCKET_APPYING_TIME = 0.5f;
			public const float BOMB_APPYING_TIME = 0.3f;
			public const float UP_POINT = 15f;
			public const float DOWN_POINT = -15f;
			public const float LEFT_POINT = 15f;
			public const float RIGHT_POINT = -15f;

			public const string BONUS_APPLIER_TAG = "BonusApplier";
		}

		public static class ScoreData
		{
			public const int DEFAULT_SCORE = 10;
			public const int DEFAULT_SCORE_WITH_BONUS = 20;
		}

		public static class MovesData
		{
			public const int MOVES_WARNING = 5;
		}

		public static class LevelData
		{
			public const string LEVEL_FORMAT = "Level {0}";
			public const string SCORE_FORMAT = "SCORE: {0}";
		}

		public static class MessageBox
		{
			public const string CONFIRM_TITLE = "CONFIRM";
			public const string CONFIRM_EXIT = "Are you sure you want to exit to the main menu?";
			public const string CONFIRM_RESTART = "Are you sure you want to restart the level?";
			public const string BUTTON_TEXT_YES = "YES";
			public const string BUTTON_TEXT_NO = "NO";
			public const string BUTTON_TEXT_OK = "OK";
			public const string BUTTON_TEXT_CANCEL = "CANCEL";
		}

		public static class HUDData
		{
			public const string MOVES_END_WINDOW = "Moves Ended";
			public const string HUD_MENU_WINDOW = "Game Menu";
		}

		public static class SettingsData
		{
			public const string MUSIC_ON = "Turn Music On";
			public const string MUSIC_OFF = "Turn Music Off";
			public const string SOUND_ON = "Turn Sound On";
			public const string SOUND_OFF = "Turn Sound Off";
		}
	}
}