namespace Game
{
	public enum BonusType
	{
		HorizontalRocket = 0,
		VerticalRocket,
		Bomb
	}

	public class Bonus
	{
		public BonusType Type { get; private set; }
		private Bonus(BonusType type)
		{
			Type = type;
		}

		public static Bonus Create(BonusType type)
		{
			return new Bonus(type);
		}
	}
}