using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class RuntimeSet<T> : ScriptableObject
{
	private HashSet<T> set = new HashSet<T>();
	public List<T> List => set.ToList();

	public int Count => set.Count;

	public void Register(T that)
	{
		if(!set.Contains(that)) {
			set.Add(that);
		}
	}

	public void Unregister(T that)
	{
		if(set.Contains(that)) {
			set.Remove(that);
		}
	}

	public bool Contains(T that)
	{
		return set.Contains(that);
	}

	public void Clear()
	{
		set.Clear();
	}
}
