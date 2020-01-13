using System;

[Serializable]
public class GameFieldColumn
{
	public int[] Columns;
}

[Serializable]
public class GameFieldRaw
{
	public GameFieldColumn[] GameField;

	public int[,] ToArray()
	{
		var size = GameField != null ? GameField.Length : 0;
		var result = new int[size, size];
		for (int indexColumn = 0; indexColumn < size; indexColumn++)
		{
			var gameFieldColumn = GameField[indexColumn];
			for (int indexRow = 0; indexRow < size; indexRow++)
			{
				result[indexRow, indexColumn] = gameFieldColumn.Columns[indexRow];
			}
		}
		return result;
	}
}