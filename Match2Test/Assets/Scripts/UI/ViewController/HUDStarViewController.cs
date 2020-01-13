using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDStarViewController : MonoBehaviour
{
    [SerializeField]
    private RectTransform _rectTransform = null;

    [SerializeField]
    private GameObject _activeStar = null;

    [SerializeField]
    private GameObject _inactiveStar = null;

    private int _scoreToActivate = 0;

    public void Init(int scoreToActivate) {
        _scoreToActivate = scoreToActivate;
        SetInactiveStar();
    }

    public void ScoreUpdate(int currentScore) {
        if (currentScore >= _scoreToActivate) {
            SetActiveStar();
        }
    }

    public void SetPosition(Vector2 position) {
        Debug.Log($"[{GetType().Name}][SeetPosition] position: {position}");
        _rectTransform.anchoredPosition = position;
    }

    public void SetActiveStar() {
        _inactiveStar.SetActive(false);
        _activeStar.SetActive(true);
    }

    public void SetInactiveStar() {
        _inactiveStar.SetActive(true);
        _activeStar.SetActive(false);
    }

    private void Start() {
        SetInactiveStar();
    }

}
