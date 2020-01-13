using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.UI
{
    public class MessageBoxViewController : IMessageBoxViewController
    {
        private readonly MessageBoxView _messageBoxView;

        private MessageBoxViewController(MessageBoxView messageBoxView) {
            _messageBoxView = messageBoxView;
            Hide();
        }

        public static IMessageBoxViewController Create(MessageBoxView messageBoxView) {
            return new MessageBoxViewController(messageBoxView);
        }

        public void Hide() {
            _messageBoxView.Hide();
        }

        public void Init(string title, string description, string firstButtonText, Action firstButtonAction) {
            _messageBoxView.Init(title, description, firstButtonText, firstButtonAction);
        }

        public void Init(string title, string description, string firstButtonText, string secondButtonText, Action firstButtonAction, Action secondButtonAction) {
            _messageBoxView.Init(title, description, firstButtonText, secondButtonText, firstButtonAction, secondButtonAction);
        }

        public void Show() {
            _messageBoxView.Show();
        }
    }

    public interface IMessageBoxViewController: IBaseUIController
    {
        void Init(string title, string description, string firstButtonText, Action firstButtonAction);
        void Init(string title, string description, string firstButtonText, string secondButtonText, Action firstButtonAction, Action secondButtonAction);
    }
}