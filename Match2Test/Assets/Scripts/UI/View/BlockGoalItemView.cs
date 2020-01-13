using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockGoalItemView : MonoBehaviour
{
    [SerializeField] private Image _blockSprite = null;
    [SerializeField] private Image _completeIcon = null;
    [SerializeField] private Text _amount = null;

    public BlockType Type => _blockGoal.Type;
    private IBlockGoal _blockGoal;

    public void Init(IBlockGoal blockGoal) {
        _blockGoal = blockGoal;
        Subscribe();
        SetAmountTextVisability(true);
        SetCompeteIconVisabilbity(false);
        _amount.text = _blockGoal.Amount.ToString();
        _blockSprite.overrideSprite = GameElementSpriteHolder.GetBlockSprite(_blockGoal.Type);
    }

    public void InitCompleted(BlockType blockType) {
        OnBlockGoalCompleteHandler(null);
        _blockSprite.overrideSprite = GameElementSpriteHolder.GetBlockSprite(blockType);
    }

    private void Subscribe() {
        _blockGoal.BlocGoalAmountChangedEvent += OnBlockGoalAmountChangeHandler;
        _blockGoal.BlockGoalCompleteEvent += OnBlockGoalCompleteHandler;
    }

    private void Unsubscribe() {
        if (_blockGoal == null)
            return;

        _blockGoal.BlocGoalAmountChangedEvent -= OnBlockGoalAmountChangeHandler;
        _blockGoal.BlockGoalCompleteEvent -= OnBlockGoalCompleteHandler;
    }

    private void OnBlockGoalCompleteHandler(IBlockGoalController obj) {
        SetCompeteIconVisabilbity(true);
        SetAmountTextVisability(false);
        Unsubscribe();
    }

    private void OnBlockGoalAmountChangeHandler(BlockMatchingEventArgs eventHandler) {
        _amount.text = _blockGoal.Amount.ToString();
    }

    private void SetCompeteIconVisabilbity(bool visible) {
        _completeIcon.gameObject.SetActive(visible);
    }

    private void SetAmountTextVisability(bool visible) {
        _amount.gameObject.SetActive(visible);
    }

    private void OnDestroy() {
        Unsubscribe();
    }

}
