using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirtinessDebug : MonoBehaviour
{
	[SerializeField]
	private Game.SpecifiedFillingEventCollection collection;

	private void Start()
	{
		for(var i = 0; i < collection.Events.Count; i++) {
			var item = collection.Events[i];
			item.OnSpecifiedFilling += (current) => Print(item, collection.Events.IndexOf(item));
	
		}
	}

	private void Print(Game.SpecifiedFillingEvent invoker, int i)
	{
		Debug.Log($"{invoker}, {i}");
	}
}
