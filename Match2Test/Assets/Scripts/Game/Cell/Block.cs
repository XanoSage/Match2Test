using System;

namespace Game
{
	public enum BlockType
	{
		Empty = 0,
		Red,
		Green,
		Blue,
		Yellow,
		//for counter
		Count
	}

	public class Block
	{
		public BlockType Type { get; private set; }

        public event Action BlockTypeChanged;

		private Block(BlockType type)
		{
			Type = type;
		}

		public static Block Create(BlockType type)
		{
			return new Block(type);
		}

		public void SetType(BlockType type)
		{
			Type = type;
            BlockTypeChanged?.Invoke();
		}
	}
}