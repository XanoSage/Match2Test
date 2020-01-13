using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public class BonusController:IBonusesController
	{
		private BonusesModel _bonusesModel;
		private BonusType[] _availableBonus;

		private BonusController() {

		}

		public static IBonusesController Create() {
			return new BonusController();
		}

		public float GetBonusApplyTime(BonusType bonusType) {
			switch (bonusType) {
				case BonusType.HorizontalRocket:
				case BonusType.VerticalRocket:
					return GameConstants.BonusData.ROCKET_APPYING_TIME;
					
				case BonusType.Bomb:
					return GameConstants.BonusData.BOMB_APPYING_TIME;
				default:
					break;
			}
			return 0f;
		}

		public void Init(BonusesModel bonusesModel, BonusType[] availableBonusTypeArray)
		{
			_bonusesModel = bonusesModel;
			_availableBonus = availableBonusTypeArray;
		}

		public bool TryGetBonuses(int count, out BonusType bonusType)
		{
			bonusType = BonusType.HorizontalRocket;

			for (int i = _bonusesModel.BonusDataList.Count - 1; i >= 0; i--)
			{
				var bonusData = _bonusesModel.BonusDataList[i];
				if (count >= bonusData.CellCount && IsBonusAvailable(bonusData.BonusTypeList))
				{
					var index =  UnityEngine.Random.Range(0, bonusData.BonusTypeList.Count);
					bonusType = bonusData.BonusTypeList[index];
					return true;
				}
			}

			return false;
		}

		private bool IsBonusAvailable(List<BonusType> bonusTypeList)
		{
			var result = _availableBonus.Any(bonusType => bonusTypeList.Any(bonusInner => bonusInner == bonusType));
			return result;
		}
	}
}