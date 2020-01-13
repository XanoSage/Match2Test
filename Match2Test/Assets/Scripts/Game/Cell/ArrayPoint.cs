namespace Game
{
	public struct ArrayPoint
	{
		public int X;
		public int Y;

		public ArrayPoint(int indexRow, int indexColumn)
		{
			X = indexRow;
			Y = indexColumn;
		}

		public override int GetHashCode() {
			return X ^ Y;
		}

		public override string ToString()
		{
			return string.Format($"({X}, {Y})");
		}
	}
}