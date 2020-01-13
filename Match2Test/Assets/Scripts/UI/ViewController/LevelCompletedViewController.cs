using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using User;

namespace Game.UI 
{
    public class LevelCompletedViewController : ILevelCompletedViewController {

        private readonly LevelCompletedView _levelCompletedView;

        public event Action OkButtonClickEvent;

        private LevelCompletedViewController(LevelCompletedView levelCompletedView) {
            _levelCompletedView = levelCompletedView;
            _levelCompletedView.OkButtonClickEvent += OnOkButtonClickHandler;
            Hide();
        }

        public static ILevelCompletedViewController Create(LevelCompletedView levelCompletedView) {
            return new LevelCompletedViewController(levelCompletedView);
        }

        public void Hide() {
            _levelCompletedView.Hide();
        }

        public void Init(LevelCompleteEventArgs levelCompleteEventArgs) {
            _levelCompletedView.Init(levelCompleteEventArgs);
        }

        public void Show() {
            _levelCompletedView.Show();
        }

        private void OnOkButtonClickHandler() {
            OkButtonClickEvent?.Invoke();
        }
        
    }

    public interface ILevelCompletedViewController: IBaseUIController {
        void Init(LevelCompleteEventArgs levelCompleteEventArgs);
        event Action OkButtonClickEvent;
    }
}