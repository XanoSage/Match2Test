using System;

namespace Game
{
	public interface IScoreController : IScoreInfo, IScoreEvents, IScore
	{
		void Init(int[] starScoreData);
		void AddScore(int blockCount, bool isBonus);
	}

	public interface IScore : IScoreInfo, IScoreEvents
	{

	}

	public interface IScoreInfo
	{
		int CurrentScore { get; }
		int StarAmount { get; }

		float ScoreProgress { get; }
		int MaxStarScore { get; }
	}

	public interface IScoreEvents
	{
		event Action<int[]> StarScoreDataInitedEvent;
		event Action<int> ScoreChangedEvent;
	}
}