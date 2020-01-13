using Game.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Game
{
	public class LevelController : ILevelController
	{
        public IScoreController ScoreController { get; }

        public IScore Score => ScoreController;

		public int StepLeft { get { return _stepCounter; } }

		public IScoreInfo ScoreInfo => ScoreController;

        public IGameFieldController GameField { get; }

		public IMovesController MovesController { get; }

		public IGoalController Goals { get; }

		public IBonusesController BonusesController { get; }

		public event LevelCompleteDelagate LevelCompletedEvent;

		private int _stepCounter = 0;
		private LevelModel _levelModel;

		private LevelController(
            IScoreController scoreController, 
            IGameFieldController gameFieldController, 
            IMovesController movesController, 
            IGoalController goalController,
			IBonusesController bonusesController)
		{
			ScoreController = scoreController;
			GameField = gameFieldController;
			MovesController = movesController;
			Goals = goalController;
			BonusesController = bonusesController;
			Subscribe();
		}

		public static ILevelController Create(
            IScoreController scoreController, 
            IGameFieldController gameFieldController, 
            IMovesController movesController, 
            IGoalController goalController,
			IBonusesController bonusesController)
		{
			return new LevelController(scoreController, gameFieldController, movesController, goalController, bonusesController);
		}

		public void Init(LevelModel level)
		{
			_levelModel = level;
			ScoreController.Init(level.StarScoreData);
			GameField.Init(_levelModel.GameFieldRawData.ToArray(), level.AvailableBlocks);
			BonusesController.Init(level.BonusModel, level.AvailableBonuses);
			MovesController.Init(level.MovesAmount, GameConstants.MovesData.MOVES_WARNING);
            Goals.Init(level.GoalData);
		}

		private void Subscribe() {
			GameField.CellMatchingEvent += OnCellMathcingEvent;
			Goals.GoalsCompleteEvent += OnGoalsCompleteHandler;
		}

		private void OnGoalsCompleteHandler() {
			RaiseLevelCompletedEvent();
		}

		private void OnCellMathcingEvent(int cellCount, bool isBonus) {
			ScoreController.AddScore(cellCount, isBonus);
			MovesController.DoMove();
		}

		private void RaiseLevelCompletedEvent() {
            var levelCompleteData = Factory.ProgressFactory.CreateLevelCompleteData(_levelModel.LevelNumber, Score.CurrentScore, Score.StarAmount, MovesController.MovesLeft);
            var blockGoalList = Goals.GetBlockGoals();

            AudioManager.PlaySoundFx(SoundFxType.LevelComplete);

			LevelCompletedEvent?.Invoke(this,  new LevelCompleteEventArgs() { CompleteData = levelCompleteData, BlockGoalsList = blockGoalList});
		}
	}	
}