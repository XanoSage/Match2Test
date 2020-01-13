using Game.Audio;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class MovesController : IMovesController
    {
        public int MovesLeft {
            get {
                return _movesLeft;
            }
            set {
                _movesLeft = value;
                RaiseMovesChangedEvent(_movesLeft);
            }
                }

        public event Action MovesEndEvent;
        public event Action<int> MovesLeftWarning;
        public event Action<int> MovesChangedEvent;

        private int _movesWarning;
        private int _movesLeft;

        private MovesController() {

        }

        public static IMovesController Create() {
            return new MovesController();
        }

        public void DoMove() {
            if (MovesLeft <= 0)
                return;

            MovesLeft--;

            if (MovesLeft == _movesWarning)
                RaiseMovesLeftWarning();
            else if (MovesLeft <= 0)
                RaiseMovesEndEvent();
        }

        public void Init(int movesCount, int movesWarning) {
            MovesLeft = movesCount;
            _movesWarning = movesWarning;
        }

        private void RaiseMovesChangedEvent(int movesLeft) {
            MovesChangedEvent?.Invoke(movesLeft);
        }

        private void RaiseMovesLeftWarning() {
            MovesLeftWarning?.Invoke(_movesWarning);
        }

        private void RaiseMovesEndEvent() {
            AudioManager.PlaySoundFx(SoundFxType.LevelFail);
            MovesEndEvent?.Invoke();
        }
    }

    public interface IMovesController: IMoves
    {
        void Init(int movesCount, int movesWarning);
        void DoMove();
    }

    public interface IMoves : IMovesInfo, IMovesEvent
    {

    }

    public interface IMovesInfo
    {
        int MovesLeft { get; }
    }

    public interface IMovesEvent
    {
        event Action MovesEndEvent;
        event Action<int> MovesLeftWarning;
        event Action<int> MovesChangedEvent;
    }
}