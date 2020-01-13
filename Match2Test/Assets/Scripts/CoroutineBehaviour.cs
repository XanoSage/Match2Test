using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineBehaviour : MonoBehaviour
{
    private static CoroutineBehaviour _instanceInner;
    private static CoroutineBehaviour _instance {
        get {
            if (_instanceInner == null) {
                var go = new GameObject("CoroutineBehaviour");
                _instanceInner = go.AddComponent<CoroutineBehaviour>();
            }

            return _instanceInner;
        }
    }
    private void Awake() {
        _instanceInner = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator DelayedActionInner(float delay, Action action) {
        yield return new WaitForSeconds(delay);
        action?.Invoke();
    }

    public static void DelayedAction(float delay, Action action) {
        _instance.StartCoroutine(_instance.DelayedActionInner(delay, action));
    }
}
