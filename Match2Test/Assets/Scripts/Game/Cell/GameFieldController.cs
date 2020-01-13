using Game.Audio;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class GameFieldController : IGameFieldController
	{
		private enum CellInteractionResult
		{
			None = 0,
			CellMatching,
			BonusApplying
		}

		private int _rowCount;
		private int _columnCount;
		private Cell[,] _gameField;

		private List<Cell> _matchingCellList = new List<Cell>();

		public event CellCreation CellCreatedFromStartEvent;
		public event CellCreation CellCreatedEvent;
		public event Action PositionChangedEvent;
		public event CellMatching CellMatchingEvent;
		public event BlockMatchingDelegate BlockMatchingEvent;

		private bool _isInteractionLocked = false;
		private readonly IBonusesController _bonusController;
		private BlockType[] _availableBlocks;

		private GameFieldController(IBonusesController bonusesController) {
			_bonusController = bonusesController;
		}

		public static IGameFieldController Create(IBonusesController bonusesController) {
			return new GameFieldController(bonusesController);
		}

		public void Clear() {
			for (int indexRow = 0; indexRow < _rowCount; indexRow++) {
				for (int indexColumn = 0; indexColumn < _columnCount; indexColumn++) {
					var cell = _gameField[indexRow, indexColumn];
					cell.Clear();
					cell.CellStateChangedEvent -= OnCellStateChangedHandler;
					cell.ChangedState(CellState.Destroy);
				}
			}
		}

		public void SetLockInteraction(bool locked) {
			_isInteractionLocked = locked;
		}

		public void DoInteraction(Cell cell) {
			Debug.Log($"[{GetType().Name}][DoInteraction] cell: {cell}");

			if (_isInteractionLocked)
				return;

			var interactionResult = TryInteract(cell);

			SetLockInteraction(true);
			
			switch (interactionResult) {
				case CellInteractionResult.None:
					OnNoneResultHandler();
					break;
				case CellInteractionResult.CellMatching:
					OnCellMatchingResultHandler(cell);
					break;
				case CellInteractionResult.BonusApplying:
					OnBonusApplyingHandler(cell);
					break;
			}

			CoroutineActionHelp.DelayedAction(0f, () => SetLockInteraction(false));
		}

		public void Init(int[,] gameFieldRawData, BlockType[] availableBlocks) {
			_availableBlocks = availableBlocks;
			_rowCount = gameFieldRawData.GetLength(0);
			_columnCount = gameFieldRawData.GetLength(1);
			_gameField = new Cell[_rowCount, _columnCount];

			//create game field from raw data
			//1-4 - it is block
			//5-7 - it is bonus
			for (int indexRow = 0; indexRow < _rowCount; indexRow++) {
				for (int indexColumn = 0; indexColumn < _columnCount; indexColumn++) {
					var cell = CreateCell(gameFieldRawData[indexRow, indexColumn], indexRow, indexColumn);
					cell.CellStateChangedEvent += OnCellStateChangedHandler;
					_gameField[indexRow, indexColumn] = cell;
					RaiseCellCreationFromStartEvent(cell);
				}
			}
		}

		//drop down the empty cell, create new cell instead of an empty cell
		private void Dropdown(bool isItBonus) {
			
			_bonusApplyCounter--;

			Debug.Log($"[{GetType().Name}][Dropdown]_bonusApplyCounter: {_bonusApplyCounter}");
			if (_bonusApplyCounter > 0)
				return;

			var needUpdatePos = false;
			var cellsToDestroyList = new List<Cell>();
			for (int indexColumn = _columnCount - 1; indexColumn >= 0; indexColumn--) {
				int emptyInColumnCounter = -1;
				for (int indexRow = 0; indexRow < _rowCount; indexRow++) {
					var cell = _gameField[indexRow, indexColumn];
					if (cell.IsMarkToDestroy || cell.IsMarkToBonus || cell.IsDestroyed) {
						needUpdatePos = true;

						if (!cell.IsDestroyed)
							cell.ChangedState(CellState.Destroy);

						cellsToDestroyList.Add(cell);
						//start shift
						ShiftElementsInColumn(cell, emptyInColumnCounter);
						emptyInColumnCounter++;
					}
				}
			}

			if (!needUpdatePos) {
				return;
			}

			RaisePositionChangedEvent();
			CreateNewCell(cellsToDestroyList);
			RaiseCellMatchingEvent(cellsToDestroyList.Count, isItBonus);
			PrepareBlockMatchingEventData(cellsToDestroyList);

			AudioManager.PlaySoundFx(SoundFxType.BlockMove);
		}

		/// <summary>
		/// Shift cell elements in column
		/// </summary>
		/// <param name="cell">started cell</param>
		/// <param name="emptyInColumnCounter">how match cell need to skip (that already shifted)</param>
		private void ShiftElementsInColumn(Cell cell, int emptyInColumnCounter) {
			emptyInColumnCounter = emptyInColumnCounter > 0 ? emptyInColumnCounter : 0;
			for (int indexRow = cell.Position.X; indexRow > emptyInColumnCounter; indexRow--) {
				var current = _gameField[indexRow, cell.Position.Y];
				var top = _gameField[indexRow - 1, cell.Position.Y];

				var tmp = _gameField[indexRow, cell.Position.Y];
				_gameField[indexRow, cell.Position.Y] = _gameField[indexRow - 1, cell.Position.Y];
				_gameField[indexRow - 1, cell.Position.Y] = tmp;

				var tmpPos = current.Position;
				current.Position = top.Position;
				top.Position = tmpPos;
			}
		}

		private void OnNoneResultHandler() {
			AudioManager.PlaySoundFx(SoundFxType.BlockPressError);
		}

		private void OnBonusApplyingHandler(Cell cell) {
			ResetBonusCounter();
			cell.ChangedState(CellState.Destroy);
			ProcessBonusApplying(cell);
		}

		private int _bonusApplyCounter = 0;
		private void ResetBonusCounter() {
			_bonusApplyCounter = 0;
		}
		private void ProcessBonusApplying(Cell cell, bool forced = false) {
			var additionalTime = 1.0f;
			var bonusApplyTime = _bonusController.GetBonusApplyTime(cell.CellBonus.Type);
			_bonusApplyCounter++;
			Debug.Log($"[{GetType().Name}][ProcessBonusApplying] _bonusApplyCounter: {_bonusApplyCounter}");
			CoroutineActionHelp.DelayedAction(bonusApplyTime * additionalTime, () => SetLockInteraction(false));
			CoroutineActionHelp.DelayedAction(GameConstants.GameData.DELAY_BEFORE_DROP_DOWN, () => Dropdown(true), forced);
		}

		private void OnCellStateChangedHandler(Cell cell) {
			
			if (cell.State != CellState.DestroyedByBonus)
				return;

			if (cell.IsItBonus) {
				Debug.Log($"[{GetType().Name}][OnCellStateChangedHandler] cell: {cell}, state: {cell.State}");
				SetLockInteraction(true);
				//CoroutineActionHelp.Clear();
				ProcessBonusApplying(cell, true);
			}
		}

		private void OnCellMatchingResultHandler(Cell cell) {
			ResetBonusCounter();
			var bonusType = BonusType.HorizontalRocket;
			if (_bonusController.TryGetBonuses(_matchingCellList.Count, out bonusType)) {
				ProcessBonusesCreation(_matchingCellList, cell, bonusType);
				CoroutineActionHelp.DelayedAction(GameConstants.BonusData.BONUS_CREATION_TIME * 0.5f, null, true);
			}
			CoroutineActionHelp.DelayedAction(0f, () => SetLockInteraction(false));
			CoroutineActionHelp.DelayedAction(GameConstants.GameData.DELAY_BEFORE_DROP_DOWN, () => Dropdown(false), true);
			AudioManager.PlaySoundFx(SoundFxType.BlockPress);
		}

		private CellInteractionResult TryInteract(Cell cell) {
			_matchingCellList.Clear();
			if (cell.IsItBonus) {
				return CellInteractionResult.BonusApplying;
			}
			// use floodfill algorithm to check if any neighbours has same cell block type
			else if (HasSameCellNeighbour(cell)) {
				var targetBlockType = cell.CellBlock.Type;
				Stack<Cell> stack = new Stack<Cell>();
				stack.Push(cell);
				var counter = 0;

				while (stack.Count > 0) {
					var cellInner = stack.Pop();
					if (cellInner.IsMarkToDestroy)
						continue;

					Cell sameCell = null;
					if (IsTopCellSame(cellInner, out sameCell)) {
						stack.Push(sameCell);
					}

					if (IsBottomCellSame(cellInner, out sameCell)) {
						stack.Push(sameCell);
					}

					if (IsLeftCellSame(cellInner, out sameCell)) {
						stack.Push(sameCell);
					}

					if (IsRightCellSame(cellInner, out sameCell)) {
						stack.Push(sameCell);
					}

					cellInner.ChangedState(CellState.MarkToDestroy);
					_matchingCellList.Add(cellInner);
					counter++;

					// to avoid stackoverflow exception
					if (counter > GameConstants.GameData.STACK_ITERATION_COUNT) {
						break;
					}
				}

				return CellInteractionResult.CellMatching;
			}
			return CellInteractionResult.None;
		}

		private void ProcessBonusesCreation(List<Cell> matchingCell, Cell bonusCell, BonusType bonusType) {
			Cell newBonusCell = null;
			foreach (var cell in matchingCell) {
				if (cell == bonusCell) {

					cell.ChangedState(CellState.Destroy);
					newBonusCell = CreateBonus(bonusType, bonusCell.Position.X, bonusCell.Position.Y);
					newBonusCell.CellStateChangedEvent += OnCellStateChangedHandler;
					_gameField[newBonusCell.Position.X, newBonusCell.Position.Y] = newBonusCell;
					
				}
				else {
					cell.ChangedState(CellState.MarkToBonus);
				}
			}

			RaiseCellCreationFromStartEvent(newBonusCell);
		}

		private Cell CreateCell(int rawGameElement, int indexRow, int indexColumn) {
			//check if this block
			if (rawGameElement < (int)BlockType.Count) {
				var block = Factory.GameElementFactory.CreateBlock((BlockType)rawGameElement);
				return Factory.CellFactory.CreateBlock(block, indexRow, indexColumn);
			}
			else // this is bonus
			{
				var bonusType = (BonusType)Mathf.Abs((int)BlockType.Count - rawGameElement);
				var bonus = Factory.GameElementFactory.CreateBonus(bonusType);
				return Factory.CellFactory.CreateBonus(bonus, indexRow, indexColumn);
			}
		}

		//use this event when level started
		private void RaiseCellCreationFromStartEvent(Cell cell) {
			CellCreatedFromStartEvent?.Invoke(cell);
		}

		private void RaiseCellCreationEvent(Cell cell) {
			CellCreatedEvent?.Invoke(cell);
		}

		private void RaisePositionChangedEvent() {
			PositionChangedEvent?.Invoke();
		}

		private void CreateNewCell(List<Cell> cellsToDestroyList) {
			foreach (var cell in cellsToDestroyList) {
				CreateRandomBlock(cell.Position.X, cell.Position.Y);
			}
		}

		private Cell CreateBonus(BonusType bonusType, int indexRow, int indexColumn) {
			var bonus = Factory.GameElementFactory.CreateBonus(bonusType);
			return Factory.CellFactory.CreateBonus(bonus, indexRow, indexColumn);
		}

		private void CreateRandomBlock(int indexRow, int indexColumn) {
			var index = UnityEngine.Random.Range(0, _availableBlocks.Length);
			var blockType = _availableBlocks[index];
			var cell = CreateCell((int)blockType, indexRow, indexColumn);
			cell.CellStateChangedEvent += OnCellStateChangedHandler;
			_gameField[indexRow, indexColumn] = cell;
			RaiseCellCreationEvent(cell);
		}


		#region check same cell using floodfill algorithm

		private bool IsTopCellSame(Cell target, out Cell top) {
			top = null;

			if (target.IsMarkToDestroy)
				return false;

			var result = false;

			if (target.Position.X - 1 >= 0) {
				top = _gameField[target.Position.X - 1, target.Position.Y];
				result = !top.IsItBonus && target.CellBlock.Type == top.CellBlock.Type;
			}

			return result;
		}

		private bool IsBottomCellSame(Cell target, out Cell bottom) {
			bottom = null;

			if (target.IsMarkToDestroy || target.IsItBonus)
				return false;

			var result = false;

			if (target.Position.X + 1 < _rowCount) {
				bottom = _gameField[target.Position.X + 1, target.Position.Y];
				result = !bottom.IsItBonus &&  target.CellBlock.Type == bottom.CellBlock.Type;
			}

			return result;
		}

		private bool IsLeftCellSame(Cell target, out Cell left) {
			left = null;

			if (target.IsMarkToDestroy)
				return false;

			var result = false;

			if (target.Position.Y - 1 >= 0) {
				left = _gameField[target.Position.X, target.Position.Y - 1];
				result = !left.IsItBonus && target.CellBlock.Type == left.CellBlock.Type;
			}

			return result;
		}

		private bool IsRightCellSame(Cell target, out Cell right) {
			right = null;

			if (target.IsMarkToDestroy)
				return false;

			var result = false;

			if (target.Position.Y + 1 < _columnCount) {
				right = _gameField[target.Position.X, target.Position.Y + 1];
				result = !right.IsItBonus && target.CellBlock.Type == right.CellBlock.Type;
			}

			return result;
		}

		private bool HasSameCellNeighbour(Cell cell) {
			bool leftSame = false;

			if (cell.Position.X - 1 >= 0) {
				leftSame = !_gameField[cell.Position.X - 1, cell.Position.Y].IsItBonus && 
					cell.CellBlock.Type == _gameField[cell.Position.X - 1, cell.Position.Y].CellBlock.Type;
			}

			bool rightSame = false;

			if (cell.Position.X + 1 < _rowCount) {
				rightSame = !_gameField[cell.Position.X + 1, cell.Position.Y].IsItBonus	&& 
					cell.CellBlock.Type == _gameField[cell.Position.X + 1, cell.Position.Y].CellBlock.Type;
			}

			bool topSame = false;

			if (cell.Position.Y - 1 >= 0) {
				topSame = !_gameField[cell.Position.X, cell.Position.Y - 1].IsItBonus && 
					cell.CellBlock.Type == _gameField[cell.Position.X, cell.Position.Y - 1].CellBlock.Type;
			}

			bool bottomSame = false;

			if (cell.Position.Y + 1 < _rowCount) {
				bottomSame = !_gameField[cell.Position.X, cell.Position.Y + 1].IsItBonus && 
					cell.CellBlock.Type == _gameField[cell.Position.X, cell.Position.Y + 1].CellBlock.Type;
			}

			return leftSame || rightSame || topSame || bottomSame;
		}

		#endregion

		private void RaiseCellMatchingEvent(int cellCount, bool isBons) {
			CellMatchingEvent?.Invoke(cellCount, isBons);
		}

		private void RaiseBlockMatchingEvent(BlockType type, int amount) {
			BlockMatchingEvent?.Invoke(new BlockMatchingEventArgs(type, amount));
		}

		private void PrepareBlockMatchingEventData(List<Cell> destroyCellList) {
			var data = new Dictionary<BlockType, int>();
			foreach (var cell in destroyCellList) {
				if (!cell.IsItBonus) {
					if (!data.ContainsKey(cell.CellBlock.Type)) {
						data[cell.CellBlock.Type] = 0;
					}
					data[cell.CellBlock.Type]++;
				}
			}

			foreach (var eventData in data) {
				RaiseBlockMatchingEvent(eventData.Key, eventData.Value);
			}
		}


	}


}