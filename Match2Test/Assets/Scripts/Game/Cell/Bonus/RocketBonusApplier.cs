using DG.Tweening;
using Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketBonusApplier : BonusApplier
{
    [SerializeField] private Transform _onePart = null;
    [SerializeField] private Transform _secondPart = null;

    public override void Apply() {
        base.Apply();
        if (_bonusType == BonusType.VerticalRocket) {
            ApplyVerticalRocket();
        }
        else if (_bonusType == BonusType.HorizontalRocket) {
            ApplyHorizontalRocket();
        }
        OnRocketApplied();
    }

    private void ApplyVerticalRocket() {
        _onePart.DOLocalMoveY(GameConstants.BonusData.UP_POINT, GameConstants.BonusData.ROCKET_APPYING_TIME).SetEase(Ease.InFlash);
        _secondPart.DOLocalMoveY(GameConstants.BonusData.DOWN_POINT, GameConstants.BonusData.ROCKET_APPYING_TIME).SetEase(Ease.InFlash);
    }

    private void ApplyHorizontalRocket() {
        _onePart.DOLocalMoveX(GameConstants.BonusData.RIGHT_POINT, GameConstants.BonusData.ROCKET_APPYING_TIME).SetEase(Ease.InFlash);
        _secondPart.DOLocalMoveX(GameConstants.BonusData.LEFT_POINT, GameConstants.BonusData.ROCKET_APPYING_TIME).SetEase(Ease.InFlash);
    }

    private void OnRocketApplied() {
        Destroy(gameObject, GameConstants.BonusData.ROCKET_APPYING_TIME);
    }
}
