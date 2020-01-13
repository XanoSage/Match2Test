using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIViewHolder : MonoBehaviour
{
    [SerializeField] private List<UIViewBase> _uiViewList = null;

    private static UIViewHolder _instantce;

    private T GetViewInner<T>() where T : UIViewBase {
        foreach (var uiView in _uiViewList) {
            if (uiView is T)
                return (T)uiView;
        }
        return null;
    }

    private void Awake() {
        _instantce = this;
    }

    public static T GetView<T>() where T : UIViewBase {
        return _instantce.GetViewInner<T>();
    }
}
