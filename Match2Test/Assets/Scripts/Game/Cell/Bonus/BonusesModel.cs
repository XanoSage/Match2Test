using System;
using System.Collections.Generic;

namespace Game
{
	[Serializable]
	public class BonusData
	{
		public int CellCount;
		public List<BonusType> BonusTypeList;
	}

	[Serializable]
	public class BonusesModel
	{
		public List<BonusData> BonusDataList;
	}
}