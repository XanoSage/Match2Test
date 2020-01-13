using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using User;

public class LevelCompletedView : UIViewBase
{
    [SerializeField] private HUDStarViewController[] _hudStarViewControllerArray = null;
    [SerializeField] private Text _scoreText = null;
    [SerializeField] private Text _levelText = null;
    [SerializeField] private Transform _parentForGoal = null;
    [SerializeField] private BlockGoalItemView _blockGoalItemViewPrefab = null;
    [SerializeField] private Button _okButton = null;

    private List<BlockGoalItemView> _blockGoalItemViewList = new List<BlockGoalItemView>();

    public event Action OkButtonClickEvent;

    public void Init(LevelCompleteEventArgs levelCompleteEventArgs) {
        SetStarCount(levelCompleteEventArgs.CompleteData.StarAmount);
        _scoreText.text = string.Format(GameConstants.LevelData.SCORE_FORMAT, levelCompleteEventArgs.CompleteData.Score);
        _levelText.text = string.Format(GameConstants.LevelData.LEVEL_FORMAT, levelCompleteEventArgs.CompleteData.LeveNumber);
        AddCompletedBlockGoals(levelCompleteEventArgs.BlockGoalsList);
    }

    protected override void OnHide() {
        base.OnHide();
        ClearGoals();
    }

    private void Start()
    {
        _okButton.onClick.AddListener(OnOKButtonClickHandler);
    }

    private void SetStarCount(int starCount) {
        var counter = 1;
        foreach (var item in _hudStarViewControllerArray) {
            if (starCount>= counter++) {
                item.SetActiveStar();
            }
            else {
                item.SetInactiveStar();
            }
        }
    }

    private void OnOKButtonClickHandler() {
        OkButtonClickEvent?.Invoke();
    }

    private void AddCompletedBlockGoals(List<BlockType> blockGoalsList) {
        foreach (var item in blockGoalsList) {
            var blockGoalItem = Instantiate(_blockGoalItemViewPrefab, _parentForGoal);
            blockGoalItem.InitCompleted(item);
            _blockGoalItemViewList.Add(blockGoalItem);
        }
    }

    private void ClearGoals() {
        while (_blockGoalItemViewList.Count > 0) {
            var blockGoalItem = _blockGoalItemViewList[0];
            _blockGoalItemViewList.Remove(blockGoalItem);
            Destroy(blockGoalItem.gameObject);
        }
    }
}
