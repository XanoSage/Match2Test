using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using User;

namespace Game
{
    public interface ILevel : ILevelInfo, ILevelEvents {
        IScore Score { get; }
    }

    public interface ILevelInfo {
        int StepLeft { get; }
        IScoreInfo ScoreInfo { get; }
    }

    public class LevelCompleteEventArgs:EventArgs {
        public LevelCompleteData CompleteData;
        public List<BlockType> BlockGoalsList;
    }

    public delegate void LevelCompleteDelagate(object sender, LevelCompleteEventArgs levelCompleteEventArgs);


    public interface ILevelEvents {
        event LevelCompleteDelagate LevelCompletedEvent;
    }

    public interface ILevelController : ILevelInfo, ILevelEvents, ILevel
	{
		IGameFieldController GameField { get; }
		IScoreController ScoreController { get; }
		IMovesController MovesController { get; }
		IGoalController Goals { get; }

        IBonusesController BonusesController { get; }
		void Init(LevelModel level);
	}
	
}