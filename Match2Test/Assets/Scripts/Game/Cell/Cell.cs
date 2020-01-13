using System;

namespace Game
{

	public enum CellState
	{
		Normal = 0,
		MarkToDestroy,
		Bonus,
		MarkToBonus,
		Destroy,
		DestroyedByBonus
	}
	public class Cell
	{
		private Block _block;
		private Bonus _bonus;
		public bool IsItBonus => _bonus != null;
		public bool IsItEmpty => _block != null && _block.Type == BlockType.Empty;
		public bool IsMarkToDestroy => State == CellState.MarkToDestroy;
		public bool IsMarkToBonus => State == CellState.MarkToBonus;
		public bool IsDestroyed => State == CellState.Destroy || State == CellState.DestroyedByBonus;
		public Block CellBlock => _block;
		public Bonus CellBonus => _bonus;
		public ArrayPoint Position;

		public CellState State { get; private set; }

		public event Action<Cell> CellStateChangedEvent;

		private Cell(Block block, int indexRow, int indexColumn)
		{
			State = CellState.Normal;
			_block = block;
			Position = new ArrayPoint(indexRow, indexColumn);
		}

		private Cell(Bonus bonus, int indexRow, int indexColumn)
		{
			State = CellState.Normal;
			_bonus = bonus;
			Position = new ArrayPoint(indexRow, indexColumn);
		}

		public void ChangedState(CellState state) {
			State = state;
			RaiseCellStateChangedEvent();
		}

		public void Clear() {
			_block = null;
			_bonus = null;
		}

		public static Cell Create(Block block, int indexRow, int indexColumn)
		{
			return new Cell(block, indexRow, indexColumn);
		}

		public static Cell Create(Bonus bonus, int indexRow, int indexColumn)
		{
			return new Cell(bonus, indexRow, indexColumn);
		}

		public override string ToString()
		{
			var cellData = IsItBonus ? "Bs:" + ((int)_bonus?.Type).ToString() : "Bl:" + ((int)_block?.Type).ToString();
			//return string.Format($"{cellData}, Point: {Position}");
			return string.Format($"{cellData} {Position}");
		}

		private void RaiseCellStateChangedEvent() {
			CellStateChangedEvent?.Invoke(this);
		}
	}

	public static partial class Factory
	{
		public static class CellFactory
		{
			public static Cell CreateBlock(Block block, int indexRow, int indexColumn)
			{
				return Cell.Create(block, indexRow, indexColumn);
			}

			public static Cell CreateBonus(Bonus bonus, int indexRow, int indexColumn)
			{
				return Cell.Create(bonus, indexRow, indexColumn);
			}
		}
	}
}