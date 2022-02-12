using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
public abstract class GenericObservableBase : ScriptableObject
{
	public abstract ScriptableObject ReferenceBase { get; }
	public abstract void ForceInvoke();
}

public abstract class GenericObservable<V> : GenericObservableBase
{
	protected abstract GenericReference<V> ValueReference { get; }

	public override ScriptableObject ReferenceBase => ValueReference;

	public event System.Action<V> onChange;

	public sealed override void ForceInvoke()
	{
		onChange?.Invoke(Value);
	}

	public V Value
	{
		get => ValueReference.Value;
		set
		{
			ValueReference.Value = value;
			onChange?.Invoke(value);
		}
	}

}

#if UNITY_EDITOR
[CustomEditor(typeof(GenericObservableBase), true)]
public class GenericObservableBaseInspector : Utility.NicelyDrawnInspector
{
	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();

		EditorGUILayout.Space();
		if(GUILayout.Button("Trigger!")) {
			var context = target as GenericObservableBase;
			context.ForceInvoke();
		}
		EditorGUILayout.Space();

	}
}
#endif
