namespace Game
{
    public interface IBonusesController
	{
		void Init(BonusesModel bonusesModel, BonusType[] availableBonusTypeArray);
		bool TryGetBonuses(int cellMatchingCount, out BonusType bonusType);
		float GetBonusApplyTime(BonusType bonusType);
	}
}