using Game.UI;
using User;

namespace Game
{
	public static partial class Factory
	{
		public static class GameElementFactory
		{
			public static Block CreateBlock(BlockType type)
			{
				return Block.Create(type);
			}

			public static Bonus CreateBonus(BonusType type)
			{
				return Bonus.Create(type);
			}
		}

		public static class ProgressFactory
		{
			public static UserProgressData CreateUserProgress()
			{
				return new UserProgressData();
			}

			public static LevelCompleteData CreateLevelCompleteData(
				int levelNumber, 
				int score, 
				int starAmount, 
				int stepAmountLeft)
			{
				return new LevelCompleteData(levelNumber, score, starAmount, stepAmountLeft);
			}
		}

		public static class LevelFactory
		{
			public static ILevelController CreateLevelController(
				IScoreController scoreController, 
				IGameFieldController gameFieldController, 
				IMovesController movesController,
				IGoalController goalController,
				IBonusesController bonusesController)
			{
				return LevelController.Create(scoreController, gameFieldController, movesController, goalController, bonusesController);
			}

			public static IGameFieldController CreateGameField(IBonusesController bonusesController) {
				return GameFieldController.Create(bonusesController);
			}

			public static IScoreController CreateScoreController() {
				return ScoreController.Create();
			}

			public static IMovesController CreateMovesController() {
				return MovesController.Create();
			}

			public static IBonusesController CreateBonusesController() {
				return BonusController.Create();
			}
		}

		public static class GameFactory
		{
			public static IGameController CreateGameController(GameFieldViewController gameFieldViewController)
			{
				return GameController.Create(gameFieldViewController);
			}
		}

		public static class UserFactory
		{
			public static IUser CreateUser()
			{
				return User.User.Create();
			}

            public static UserSettings CreateUserSettings(bool isMusicOn, bool isSoundOn) {
                return new UserSettings(isMusicOn, isSoundOn);
            }
        }

		public static class GoalFactory
		{
			public static IGoalController CreateGoalController(IBlockMatchingEvent blockMatchingEvent) {
				return GoalController.Create(blockMatchingEvent);
			}

			public static IBlockGoalController CreateBlockGoalController(BlockGoalModel blockGoalModel, IBlockMatchingEvent blockMatchingEvent) {
				return BlockGoalController.Create(blockGoalModel, blockMatchingEvent);
			}
		}

		public static partial class ViewFactory
		{
			public static IHUDViewController CreateHUDViewController(IScore score, IMoves moves, IGoal goal, HUDView hudView) {
				return HUDViewController.Create(score, moves, goal, hudView);
			}

			public static IMessageBoxViewController CreateMessageBoxViewController(MessageBoxView messageBoxView) {
				return MessageBoxViewController.Create(messageBoxView);
			}

			public static IMainMenuViewController CreateMainMenuViewController(MainMenuView mainMenuView) {
				return MainMenuViewController.Create(mainMenuView);
			}

			public static ISettingsViewController CreateSettingsViewController(SettingsView settingsView) {
				return SettingsViewController.Create(settingsView);
			}

			public static IMovesEndedViewController CreateMovesEndedViewController(MovesEndedView movesEndedView) {
				return MovesEndedViewController.Create(movesEndedView);
			}

            public static ILevelCompletedViewController CreateLevelViewController(LevelCompletedView levelCompletedView) {
                return LevelCompletedViewController.Create(levelCompletedView);
            }

            public static IHUDMenuViewController CreateHUDMenuViewController(HUDMenuView hudMenuView) {
                return HUDMenuViewController.Create(hudMenuView);
            }
		}
	}
}