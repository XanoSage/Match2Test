using Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDView : UIViewBase
{
    [SerializeField] private Button _gameMenuButton = null;
    [SerializeField] private Text _scoreText = null;

    [SerializeField] private HUDStarViewController[] _hUDStarViewControllerArray = null;

    [SerializeField] private Slider _scoreProgressSlider = null;
    [SerializeField] private RectTransform _scoreProgressRectTransform = null;

    [SerializeField] private Text _movesLeftText = null;

    [SerializeField] private BlockGoalItemView _blockGoalItemViewPrefab = null;
    [SerializeField] private Transform _parentForGoal = null;

    private List<BlockGoalItemView> _blockGoalItemViewList = new List<BlockGoalItemView>();

    public event Action GameMenuButtonClickEvent;

    public void InitStarScoreData(int[] starScoreData, int maxScore) {
        for (int i = 0; i < starScoreData.Length; i++) {
            _hUDStarViewControllerArray[i].Init(starScoreData[i]);
            var position = GetStarPosition(starScoreData[i], maxScore);
            _hUDStarViewControllerArray[i].SetPosition(position);
        }
    }

    public void AddBlockGoal(IBlockGoal blockGoal) {
        var blockGoalViewItem = CreateBlockGoalItem(blockGoal);
        _blockGoalItemViewList.Add(blockGoalViewItem);
    }

    public void RemoveBlockGoal(IBlockGoal blockGoal) {
        var blockGoalItemView = _blockGoalItemViewList.Find(blockGoalView=> blockGoalView.Type == blockGoal.Type);
        if (blockGoalItemView == null)
            return;

        _blockGoalItemViewList.Remove(blockGoalItemView);
        Destroy(blockGoalItemView.gameObject);
    }

    public void SetScore(int score, float scoreProgress) {
        SetScoreText(score);
        UpdateStarStatus(score);
        _scoreProgressSlider.value = scoreProgress;
    }

    public void SetMovesLeftText(int movesLeft) {
        _movesLeftText.text = movesLeft.ToString();
    }

    private void Awake() {
        _gameMenuButton.onClick.AddListener(OnGameMenuButtonClickHandler);
        SetScoreText(0);
        SetMovesLeftText(0);
    }

    private void OnGameMenuButtonClickHandler() {
        GameMenuButtonClickEvent?.Invoke();
    }  

    private void UpdateStarStatus(int score) {
        foreach (var hUDStar in _hUDStarViewControllerArray) {
            hUDStar.ScoreUpdate(score);
        }
    }

    private void SetScoreText(int score) {
        _scoreText.text = score.ToString();
    }

    private Vector2 GetStarPosition(int score, int maxScore) {
        var scoreProgress = (float)score / (float)maxScore;
        var result = Vector2.zero;
        var size = _scoreProgressRectTransform.rect.size;
        result.x = size.x * scoreProgress;
        return result;
    }

    private BlockGoalItemView CreateBlockGoalItem(IBlockGoal blockGoal) {
        var blockGoalView = Instantiate(_blockGoalItemViewPrefab, _parentForGoal);
        blockGoalView.Init(blockGoal);
        return blockGoalView;
    }
}
