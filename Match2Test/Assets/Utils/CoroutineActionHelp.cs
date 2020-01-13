using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineActionHelp : MonoBehaviour
{
	private Queue<CoroutineActionData> _queue = new Queue<CoroutineActionData>();
	private static CoroutineActionHelp _instanceInner;

	private static CoroutineActionHelp _instance {
		get {
			if (_instanceInner == null) {
				var go = new GameObject("CoroutineActionHelp");
				_instanceInner = go.AddComponent<CoroutineActionHelp>();
			}

			return _instanceInner;
		}
	}

	private bool _isInProcess = false;
	private void Awake()
    {
        _instanceInner = this;
    }
    
    public static void DelayedAction(float delay, Action action, bool forced = false)
    {
		_instance?.AddActionToQueue(delay, action, forced);
    }

	public static void Clear() {
		_instance.ClearInner();
	}

	private void ClearInner() {
		_queue.Clear();
		StopAllCoroutines();
	}

    private  IEnumerator DelayedActionInner(float delay, Action action, bool forced)
    {
		_isInProcess = true;
        yield return new WaitForSeconds(delay);
        action?.Invoke();
		_isInProcess = false;
		_instance.ProcessQueue(forced);
    }
    
	private void ProcessQueue(bool forced = false)
	{
		if (_queue.Count == 0 || _isInProcess && !forced)
			return;

		var coroutineData = _queue.Dequeue();
		StartCoroutine(DelayedActionInner(coroutineData.Delay, coroutineData.Action, coroutineData.Forced));
	}

	private void AddActionToQueue(float delay, Action action, bool forced)
	{
		var coroutineAction = new CoroutineActionData(delay, action, forced);
		_queue.Enqueue(coroutineAction);
		ProcessQueue(forced);
	}
}

public class CoroutineActionData
{
	public float Delay;
	public Action Action;
	public bool Forced;

	public CoroutineActionData(float delay, Action action, bool forced)
	{
		Delay = delay;
		Action = action;
		Forced = forced;
	}
}
