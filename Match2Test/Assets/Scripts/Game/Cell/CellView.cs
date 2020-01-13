using DG.Tweening;
using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellView : MonoBehaviour
{
	public Cell Cell {get; private set;}
	public event Action<CellView> CellViewClicked;
	public event Action<CellView> CellViewDestroy;
    // Start is called before the first frame update

    private SpriteRenderer _spriteRenderer;

    public void Init(Cell cell) {
        Cell = cell;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Cell.CellStateChangedEvent += OnCellStateChangedHandler;
    }
	public void UpdatePosition(Vector2 startPoint, Vector2 spacing)
	{
        var newPos = new Vector3(startPoint.x + (spacing.x * Cell.Position.Y), startPoint.y - spacing.y * Cell.Position.X, 0f);
        transform.DOLocalMove(newPos, GameConstants.GameData.ANIMATION_DURATION).SetEase(Ease.OutBounce); 
	}

    public void ToBonusPosition(Vector3 bonusPosition) {
        
        transform.DOLocalMove(bonusPosition, GameConstants.BonusData.BONUS_CREATION_TIME).SetEase(Ease.OutQuad);
        _spriteRenderer.DOFade(0f, GameConstants.BonusData.BONUS_CREATION_TIME);
    }

    private void OnDestroy() {
        Cell.CellStateChangedEvent -= OnCellStateChangedHandler;
    }

    private void OnMouseDown()
	{
		CellViewClicked?.Invoke(this);
	}
    
    private void OnTriggerEnter2D(Collider2D collision) {
        //Debug.Log($"[{GetType().Name}][OnTriggerEnter] Cell: {Cell}, other: {collision.name}, battle tag = {collision.tag}");
        if (collision.CompareTag(GameConstants.BonusData.BONUS_APPLIER_TAG)) {
            Cell.ChangedState(CellState.DestroyedByBonus);
        }
    }

    private void OnCellStateChangedHandler(Cell cell) {
        if (cell.State == CellState.MarkToDestroy) {
            _spriteRenderer.enabled = false;
        }
        else if (cell.State == CellState.MarkToBonus) {
            _spriteRenderer.enabled = true;
        }
        else if (cell.State == CellState.Destroy || cell.State == CellState.DestroyedByBonus) {
            CellViewDestroy?.Invoke(this);
        }
    }
}
