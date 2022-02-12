using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericEvent<T> : EventBase
{
	public event System.Action<T> onEvent;

	public void Invoke(T parameter)
	{
		onEvent?.Invoke(parameter);
	}
}
