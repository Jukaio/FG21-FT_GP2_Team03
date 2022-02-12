using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

#endif

[CreateAssetMenu(fileName = "Void Event", menuName = "Scriptable Objects/Gameplay/Event/Void Event")]
public class VoidEvent : EventBase
{
	public event System.Action onEvent;

	public void Invoke()
	{
		onEvent?.Invoke();
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(EventBase), true)]
public class VoidListenerInspector : Editor
{
	public override void OnInspectorGUI()
	{

	}
}
#endif

