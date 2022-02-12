using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericObserver<T> : MonoBehaviour
{
	protected abstract GenericObservable<T> Observable { get; }

	public T Value
	{
		get => Observable.Value;
		set => Observable.Value = value;
	}

	protected abstract void OnChange(T value);

	private void OnEnable()
	{
		Observable.onChange += OnChange;
		Observable.Value = Value; // "Mimic" initial call
	}

	private void OnDisable()
	{
		Observable.onChange -= OnChange;
	}
}
