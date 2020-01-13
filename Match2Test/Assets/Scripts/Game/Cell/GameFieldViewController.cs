using Game;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameFieldViewController : MonoBehaviour
{
	[SerializeField] private Vector2 _startPoint = Vector2.zero;
	[SerializeField] private Vector2 _spacing = Vector2.zero;

	[SerializeField] private GameElementHolder _gameElementHolder = null;
	[SerializeField] private Transform _parentForGameElement = null;

	[SerializeField] private Transform _topPoint = null;

	private int _columnLength;
	private int _rowLength;

	private List<CellView> _cellViewList = new List<CellView>();

	private IGameFieldController _gameFieldController;

	public void Init(IGameFieldController gameFieldController)
	{
		_gameFieldController = gameFieldController;
		_gameFieldController.CellCreatedFromStartEvent += OnCellCreatedFromStartHandler;
		_gameFieldController.CellCreatedEvent += OnCellCreatedHandler;
		_gameFieldController.PositionChangedEvent += OnCellPositionChangedHandler;
	}

	public void InitGameField(int rowLength, int columnLength)
	{
		_columnLength = columnLength;
		_rowLength = rowLength;
	}

	private void OnCellCreatedFromStartHandler(Cell cell)
	{
		CreateCellView(cell);
		if (cell.IsItBonus) {
			ProcessBonusCreatedCell(_cellViewList.Last());
		}
	}

	private void OnCellCreatedHandler(Cell cell) {
		CreateCellView(cell, true);
	}

	private void CreateCellView(Cell cell, bool fromTop = false) {
		GameObject gameElementPrefab = null;
		var cellName = string.Empty;
		if (cell.IsItBonus) {
			gameElementPrefab = _gameElementHolder.GetBonusView(cell.CellBonus.Type).gameObject;
			cellName = cell.CellBonus.Type.ToString();
		}
		else {
			gameElementPrefab = _gameElementHolder.GetBlockView(cell.CellBlock.Type).gameObject;
			cellName = cell.CellBlock.Type.ToString();
		}

		var gameElement = Instantiate(gameElementPrefab, _parentForGameElement);
		var position = new Vector3(_startPoint.x + (_spacing.x * cell.Position.Y), _startPoint.y - _spacing.y * cell.Position.X, 0f);

		if (fromTop)
			position = new Vector3(_startPoint.x + (_spacing.x * cell.Position.Y), _topPoint.position.y - _spacing.y * cell.Position.X, 0f);

		gameElement.transform.localPosition = position;
		var cellView = gameElement.AddComponent<CellView>();

		cellView.CellViewClicked += OnCellClickedHandler;
		cellView.CellViewDestroy += OnCellViewDestroyHandler;
		cellView.name = cellName + "_" + cell.Position.ToString();

		cellView.Init(cell);
		_cellViewList.Add(cellView);

		if (fromTop) {
			cellView.UpdatePosition(_startPoint, _spacing);
		}
	}

	private void OnCellClickedHandler(CellView cellView)
	{
		_gameFieldController.DoInteraction(cellView.Cell);
	}

	private void OnCellPositionChangedHandler()
	{
		foreach (var cell in _cellViewList)
		{
			cell.UpdatePosition(_startPoint, _spacing);
		}
	}

	private void OnCellViewDestroyHandler(CellView cellView) {
		cellView.CellViewDestroy -= OnCellViewDestroyHandler;
		cellView.CellViewClicked -= OnCellClickedHandler;
		_cellViewList.Remove(cellView);

		if (cellView.Cell.IsItBonus)
			CreateBonusApplier(cellView);
		Destroy(cellView.gameObject);
	}

	private void CreateBonusApplier(CellView cell) {
		var bonusApplierPrefab = _gameElementHolder.GetBonusApplier(cell.Cell.CellBonus.Type);
		var bonusApplier = Instantiate(bonusApplierPrefab, _parentForGameElement);
		bonusApplier.transform.localPosition = cell.transform.localPosition;
		bonusApplier.Apply();
	}

	private void ProcessBonusCreatedCell(CellView cell) {
		foreach (var cellView in _cellViewList) {
			if (cellView == cell)
				continue;
			if (cellView.Cell.State == CellState.MarkToBonus) {
				cellView.ToBonusPosition(cell.transform.localPosition);
			}
		}
	}

	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
