using System;

namespace Game
{
	public delegate void CellCreation(Cell cell);
	public delegate void CellMatching(int cellCount, bool isBonus);

	public interface IGameFieldController : IBlockMatchingEvent
	{
		void Init(int[,] gameFieldRawData, BlockType[] availableBlocks);
		void DoInteraction(Cell cell);
		void Clear();

		void SetLockInteraction(bool locked);
		event CellCreation CellCreatedFromStartEvent;
		event CellCreation CellCreatedEvent;
		event Action PositionChangedEvent;
		event CellMatching CellMatchingEvent;
	}

	public interface IBlockMatchingEvent
	{
		event BlockMatchingDelegate BlockMatchingEvent;
	}
}