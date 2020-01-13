using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.UI
{
    public class HUDViewController : IHUDViewController
    {
        private readonly IScore _score;
        private readonly IMoves _moves;
        private readonly IGoal _goals;
        private readonly HUDView _hudView;

        public event Action GameMenuButtonClickEvent;

        private HUDViewController(IScore score, IMoves moves, IGoal goal, HUDView hudView) {
            _score = score;
            _hudView = hudView;
            _moves = moves;
            _goals = goal;
            Subscribe();
            Hide();
        }

        public static IHUDViewController Create(IScore score, IMoves moves, IGoal goal, HUDView hudView) {
            return new HUDViewController(score, moves, goal, hudView);
        }

        public void Show() {
            _hudView.Show();
        }

        public void Hide() {
            _hudView.Hide();
        }

        private void Subscribe() {
            _score.StarScoreDataInitedEvent += OnStarScoreDataInitedHandler;
            _score.ScoreChangedEvent += OnScoreChangedHandler;
            _hudView.GameMenuButtonClickEvent += OnGameMenuButtonClickHandler;
            _moves.MovesChangedEvent += OnMovesChangedHandler;
            _moves.MovesLeftWarning += OnMovesLeftWarningHandler;

            _goals.BlockGoalAddedEvent += OnBlockGoalAddedHandler;
            _goals.BlockGoalRemovedEvent += OnBlockGoalRemovedHandler;
        }

        private void OnBlockGoalAddedHandler(IBlockGoal blockGoal) {
            _hudView.AddBlockGoal(blockGoal);
        }

        private void OnBlockGoalRemovedHandler(IBlockGoal blockGoal) {
            _hudView.RemoveBlockGoal(blockGoal);
        }

        private void OnGameMenuButtonClickHandler() {
            GameMenuButtonClickEvent?.Invoke();
        }

        private void OnStarScoreDataInitedHandler(int[] starScoreData) {
            _hudView.InitStarScoreData(starScoreData, _score.MaxStarScore);
        }

        private void OnScoreChangedHandler(int score) {

            _hudView.SetScore(score, _score.ScoreProgress);
        }

        private void OnMovesChangedHandler(int movesLeft) {
            _hudView.SetMovesLeftText(movesLeft);
        }

        private void OnMovesLeftWarningHandler(int movesWarning) {
            //show warning message
        }
    }

    public interface IHUDViewController:IBaseUIController
    {
        event Action GameMenuButtonClickEvent;
    }
}