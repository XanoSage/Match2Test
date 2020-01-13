using System;
using System.Collections.Generic;
using System.Linq;

namespace User
{
	[Serializable]
	public class UserProgressData
	{
		public List<LevelCompleteData> LevelCompletedList;
		public int LastLevelCompletedNumber
		{
			get
			{
				return LevelCompletedList.Count > 0 ? LevelCompletedList.Last().LeveNumber : 0;
			}
		}

		public UserProgressData()
		{
			LevelCompletedList = new List<LevelCompleteData>();
		}

		public LevelCompleteData GetLevelCompleteData(int levelNumber)
		{
			var result = LevelCompletedList.Find(completeData=> completeData.LeveNumber == levelNumber);
			return result;
		}

		public bool TryAddLevelCompleteData(LevelCompleteData levelCompleteData)
		{
			if (LevelCompletedList.Any(completeData => completeData.LeveNumber == levelCompleteData.LeveNumber))
				return false;

			LevelCompletedList.Add(levelCompleteData);
			return true;
		}
	}
}
