using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class MovesEndedViewController : IMovesEndedViewController
    {
        private readonly MovesEndedView _movesEndedView;

        public event Action MainMenuButtonClickEvent;
        public event Action RestartButtonClickEvent;

        private MovesEndedViewController(MovesEndedView movesEndedView) {
            _movesEndedView = movesEndedView;
            _movesEndedView.MainMenuButtonClickEvent += OnMainMenuButtonClickHandler;
            _movesEndedView.RestartButtonClickEvent += OnRestartButtonClickHandler;

            Hide();
        }
        
        public static IMovesEndedViewController Create(MovesEndedView movesEndedView) {
            return new MovesEndedViewController(movesEndedView);
        }

        public void Init(string title) {
            _movesEndedView.Init(title);
        }

        public void Hide() {
            _movesEndedView.Hide();
        }

        public void Show() {
            _movesEndedView.Show();
        }

        private void OnMainMenuButtonClickHandler() {
            MainMenuButtonClickEvent?.Invoke();
        }

        private void OnRestartButtonClickHandler() {
            RestartButtonClickEvent?.Invoke();
        }
    }

    public interface IMovesEndedViewController : IBaseUIController
    {
        void Init(string title);

        event Action MainMenuButtonClickEvent;
        event Action RestartButtonClickEvent;
    }

    public interface IBaseUIController
    {
        void Show();
        void Hide();
    }
}