using System;
using System.Linq;

namespace Game
{
	public class ScoreController : IScoreController
	{
		public int CurrentScore
		{
			get
			{
				return _currentScore;
			}
			private set
			{
				_currentScore = value;
				RaiseScoreChangedEvent(_currentScore);
			}
		}

		public int StarAmount
		{
			get
			{
				if (_starScoreData == null)
					return 0;

				if (_currentScore >= _starScoreData[2])
					return 3;

				if (_currentScore >= _starScoreData[1])
					return 2;

				if (_currentScore >= _starScoreData[0])
					return 1;
				return 0;
			}
		}

		public float ScoreProgress {
			get {
				if (_currentScore > MaxStarScore)
					return 1f;
				return (float)_currentScore / (float)MaxStarScore;
			}
		}

		public int MaxStarScore { get; private set; }

		private int _currentScore = 0;
		private int[] _starScoreData;

		public event Action<int[]> StarScoreDataInitedEvent;
		public event Action<int> ScoreChangedEvent;

		private ScoreController()
		{

		}

		public static IScoreController Create()
		{
			return new ScoreController();
		}

		public void AddScore(int blockCount, bool isBonus)
		{
			var scoreSum = 0;
			var elementScore = 0; ;
			for (int i = 0; i < blockCount; i++)
			{
				if (isBonus)
				{
					scoreSum += GameConstants.ScoreData.DEFAULT_SCORE_WITH_BONUS;
				}
				else
				{
					elementScore += GameConstants.ScoreData.DEFAULT_SCORE;
					scoreSum += elementScore;
				}
			}
			CurrentScore += scoreSum;
		}

		public void Init(int[] starScoreData)
		{
			MaxStarScore = starScoreData.Last();
			CurrentScore = 0;
			_starScoreData = starScoreData;
			StarScoreDataInitedEvent?.Invoke(starScoreData);
		}

		private void RaiseScoreChangedEvent(int score)
		{
			ScoreChangedEvent?.Invoke(score);
		}
	}
}