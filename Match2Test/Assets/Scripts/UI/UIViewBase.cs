using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewBase : MonoBehaviour
{
    [SerializeField] private RectTransform _baseRectTransform = null;

    public void Show() {
        _baseRectTransform.gameObject.SetActive(true);
        OnShow();
    }

    public void Hide() {
        OnHide();
        _baseRectTransform.gameObject.SetActive(false);
    }

    protected virtual void OnShow() {

    }

    protected virtual void OnHide() {

    }
}
