using System;

namespace User
{
	[Serializable]
	public class LevelCompleteData
	{
		public int LeveNumber;
		public int Score;
		public int StarAmount;
		public int StepAmountLeft;

		public LevelCompleteData(int levelNumber, int score, int starAmount, int stepAmountLeft)
		{
			LeveNumber = levelNumber;
			Score = score;
			StarAmount = starAmount;
			StepAmountLeft = stepAmountLeft;
		}
	}
}
