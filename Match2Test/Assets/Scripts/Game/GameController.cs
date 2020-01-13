using Game.Audio;
using Game.UI;
using System.Collections.Generic;
using UnityEngine;
using User;

namespace Game {
    public class GameController : IGameController
	{
		public ILevelController Level { get; private set; }
		public IUser User { get; private set; }

		private IHUDViewController _hudViewController;
		private IMessageBoxViewController _messageBoxViewController;
		private IMainMenuViewController _mainMenuViewController;
		private ISettingsViewController _settingsViewController;
		private IMovesEndedViewController _movesEndedViewController;
        private ILevelCompletedViewController _levelCompletedViewController;
        private IHUDMenuViewController _hudMenuViewController;

		private readonly GameFieldViewController _gameFieldViewController;

		private readonly Stack<IBaseUIController> _uiViewControllerStack = new Stack<IBaseUIController>();

		private GameController(GameFieldViewController gameFieldViewController)
		{
			_gameFieldViewController = gameFieldViewController;
			CreateUser();
			CreateLevelController();
			CreateHUDViewController();
			CreateMainMenuViewController();
			CreateSettingsViewController();
			CreateMessageBoxViewController();
			CreateMovesEndedController();
            CreateLevelCompleteViewController();
            CreateHUDMenuViewController();
		}

		public static IGameController Create(GameFieldViewController gameFieldViewController)
		{
			return new GameController(gameFieldViewController);
		}

		public void Init()
		{
			_gameFieldViewController.Init(Level.GameField);
			User.Init();
			AudioManager.SetMusicEnabled(User.Settings.IsMusicOn);
			AudioManager.SetSoundEnabled(User.Settings.IsSoundOn);
			AudioManager.PlayMusic(MusicType.BackgroundMsic_1);
		}

		public void StartNextLevel()
		{
			var nextLevelNumber = User.LastCompletedLevel + 1;
			var levelModel = LevelModel.LoadLevel(nextLevelNumber);
			var gameFieldRawData = levelModel.GameFieldRawData.ToArray();
			var rowLength = gameFieldRawData.GetLength(0);
			var columnLength = gameFieldRawData.GetLength(1);
			_gameFieldViewController.InitGameField(rowLength, columnLength);
			Level.Init(levelModel);
			_hudViewController.Show();
		}

		private void CreateLevelController()
		{
			var scoreController = Factory.LevelFactory.CreateScoreController();
			var bonusController = Factory.LevelFactory.CreateBonusesController();
			var gameField = Factory.LevelFactory.CreateGameField(bonusController);
			var movesController = Factory.LevelFactory.CreateMovesController();
			movesController.MovesEndEvent += OnMovesEndHandler;
			var goalsController = Factory.GoalFactory.CreateGoalController(gameField);
			Level = Factory.LevelFactory.CreateLevelController(scoreController, gameField, movesController, goalsController, bonusController);
			Level.LevelCompletedEvent += OnLevelCompletedHandler;
		}

		private void CreateUser()
		{
			User = Factory.UserFactory.CreateUser();
		}


		private void CreateHUDViewController() {
			_hudViewController = Factory.ViewFactory.CreateHUDViewController(Level.Score, Level.MovesController, Level.Goals, UIViewHolder.GetView<HUDView>());
			_hudViewController.GameMenuButtonClickEvent += OnHUDGameMenuButtonClickHandler;
		}

		private void CreateMessageBoxViewController() {
			_messageBoxViewController = Factory.ViewFactory.CreateMessageBoxViewController(UIViewHolder.GetView<MessageBoxView>());
		}

		private void CreateMainMenuViewController() {
			_mainMenuViewController = Factory.ViewFactory.CreateMainMenuViewController(UIViewHolder.GetView<MainMenuView>());

			_mainMenuViewController.PlayButtonClickEvent += OnMainMenuPlayButtonClickHandler;
			_mainMenuViewController.SettingsButtonClickEvent += OnMainMenuSettingsButtonClickHandler;
		}

        private void CreateSettingsViewController() {
            _settingsViewController = Factory.ViewFactory.CreateSettingsViewController(UIViewHolder.GetView<SettingsView>());

            _settingsViewController.MusicSettingsChangedEvent += OnSettingsMusicSettingChangedHandler;
            _settingsViewController.SoundSettingsChangedEvent += OnSettingsSoundSettingsChangedHandler;
            _settingsViewController.OkButtonClickEvent += OnSettingsOkButtonClickHandler;
        }

        private void CreateMovesEndedController() {
            _movesEndedViewController = Factory.ViewFactory.CreateMovesEndedViewController(UIViewHolder.GetView<MovesEndedView>());
            _movesEndedViewController.MainMenuButtonClickEvent += OnMovesEndMainMenuButtonClickHandler;
            _movesEndedViewController.RestartButtonClickEvent += OnMovesEndRestartButtonClickhandler;
        }

        private void CreateLevelCompleteViewController() {
            _levelCompletedViewController = Factory.ViewFactory.CreateLevelViewController(UIViewHolder.GetView<LevelCompletedView>());
            _levelCompletedViewController.OkButtonClickEvent += OnLevelCompleteWindowOkButtonClickHandler;
        }

        private void CreateHUDMenuViewController() {
            _hudMenuViewController = Factory.ViewFactory.CreateHUDMenuViewController(UIViewHolder.GetView<HUDMenuView>());
            _hudMenuViewController.MainMenuButtonClickEvent += OnHUDMenuMainMenuButtonClickHandler;
            _hudMenuViewController.SettingsMenuButtonClickEvent += OnHUDMenuSettingsMenuButtonClickHandler;
            _hudMenuViewController.RestartButtonClickEvent += OnHUDMenuRestartButtonClickHandler;
            _hudMenuViewController.CloseButtonClickEvent += OnHUDMenuCloseButtonClickHandler;
        }

        private void OnLevelCompletedHandler(object sender, LevelCompleteEventArgs levelCompleteEventArgs) {
            Level.GameField.SetLockInteraction(true);
            _levelCompletedViewController.Init(levelCompleteEventArgs);
            User.AddLevelCompleteData(levelCompleteEventArgs.CompleteData);
            _levelCompletedViewController.Show();
        }

        private void OnMovesEndHandler() {
            Level.GameField.SetLockInteraction(true);
			_movesEndedViewController.Init(GameConstants.HUDData.MOVES_END_WINDOW);
            _movesEndedViewController.Show();
        }

        private void OnMainMenuSettingsButtonClickHandler() {
			Debug.Log($"[{GetType().Name}][OnMainMenuSettingsButtonClickHandler] Ok");
			_mainMenuViewController.Hide();
			_settingsViewController.Init(User.Settings.IsMusicOn, User.Settings.IsSoundOn);
			_settingsViewController.Show();

			_uiViewControllerStack.Push(_mainMenuViewController);
		}

		private void OnMainMenuPlayButtonClickHandler() {
			_mainMenuViewController.Hide();
			StartNextLevel();
		}

		private void OnSettingsOkButtonClickHandler() {
			_settingsViewController.Hide();
			var baseUIController = _uiViewControllerStack.Pop();
			baseUIController?.Show();
		}

		private void OnSettingsSoundSettingsChangedHandler(bool enabled) {
			Debug.Log($"[{GetType().Name}][SettingsWindow] Sound is: {enabled}");
            User.SetSoundActivity(enabled);
			AudioManager.SetSoundEnabled(enabled);
            AudioManager.PlaySoundFx(SoundFxType.ButtonClick);
        }

		private void OnSettingsMusicSettingChangedHandler(bool enabled) {
			Debug.Log($"[{GetType().Name}][SettingsWindow] Music is: {enabled}");
            User.SetMusicActivity(enabled);
            AudioManager.SetMusicEnabled(enabled);
            AudioManager.PlaySoundFx(SoundFxType.ButtonClick);
		}

		private void OnMovesEndRestartButtonClickhandler() {
			_uiViewControllerStack.Push(_movesEndedViewController);
			_movesEndedViewController.Hide();
            ShowMessageBoxConfirmRestart();
		}

        private void ShowMessageBoxConfirmRestart() {
            _messageBoxViewController.Init(
                GameConstants.MessageBox.CONFIRM_TITLE,
                GameConstants.MessageBox.CONFIRM_RESTART,
                GameConstants.MessageBox.BUTTON_TEXT_NO,
                GameConstants.MessageBox.BUTTON_TEXT_YES,
                OnMessageBoxDeclineExitToMainMenu,
                OnMessageBoxRestartConfirm
                );

            _messageBoxViewController.Show();
        }

		private void OnMovesEndMainMenuButtonClickHandler() {
			_uiViewControllerStack.Push(_movesEndedViewController);
			_movesEndedViewController.Hide();
            ShowMessageBoxConfirmToMainMenu();
		}

        private void ShowMessageBoxConfirmToMainMenu() {
            _messageBoxViewController.Init(
                GameConstants.MessageBox.CONFIRM_TITLE,
                GameConstants.MessageBox.CONFIRM_EXIT,
                GameConstants.MessageBox.BUTTON_TEXT_NO,
                GameConstants.MessageBox.BUTTON_TEXT_YES,
                OnMessageBoxDeclineExitToMainMenu,
                OnMessageBoxConfirmExitToMainMenu
                );

            _messageBoxViewController.Show();
        }

		private void OnMessageBoxConfirmExitToMainMenu() {
			_uiViewControllerStack.Pop();
			_messageBoxViewController.Hide();
			_mainMenuViewController.Show();
			_hudViewController.Hide();
            ClearLevel();
			Level.GameField.SetLockInteraction(false);
		}

		private void OnMessageBoxDeclineExitToMainMenu() {
			var baseUIController = _uiViewControllerStack.Pop();
			baseUIController.Show();
			_messageBoxViewController.Hide();
		}

		private void OnMessageBoxRestartConfirm() {
			_uiViewControllerStack.Pop();
			_messageBoxViewController.Hide();
            ClearLevel();
            Level.GameField.SetLockInteraction(false);
            StartNextLevel();
		}

        private void ClearLevel() {
            Level.GameField.Clear();
            Level.Goals.Clear();
        }

        private void OnLevelCompleteWindowOkButtonClickHandler() {
            _levelCompletedViewController.Hide();
            ClearLevel();
            Level.GameField.SetLockInteraction(false);
            StartNextLevel();
        }

		private void OnHUDGameMenuButtonClickHandler() {
			Level.GameField.SetLockInteraction(true);
            _hudMenuViewController.Show();
		}


        private void OnHUDMenuMainMenuButtonClickHandler() {
            Debug.Log($"[{GetType().Name}][OnHUDMenuMainMenuButtonClickHandler] OK");
            _uiViewControllerStack.Push(_hudMenuViewController);
            _hudMenuViewController.Hide();
            ShowMessageBoxConfirmToMainMenu();
        }

        private void OnHUDMenuSettingsMenuButtonClickHandler() {
            _uiViewControllerStack.Push(_hudMenuViewController);
            _hudMenuViewController.Hide();
            _settingsViewController.Show();
        }

        private void OnHUDMenuRestartButtonClickHandler() {
            _uiViewControllerStack.Push(_movesEndedViewController);
            _uiViewControllerStack.Push(_hudMenuViewController);
            _hudMenuViewController.Hide();
            ShowMessageBoxConfirmRestart();
        }

        private void OnHUDMenuCloseButtonClickHandler() {
            Level.GameField.SetLockInteraction(false);
            _hudMenuViewController.Hide();
        }
    }
}
