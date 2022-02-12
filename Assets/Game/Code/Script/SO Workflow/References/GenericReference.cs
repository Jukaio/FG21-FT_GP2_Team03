using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GenericReference<V> : ScriptableObject
{
	[SerializeField]
	private V value;
	public V Value
	{
		get => this.value;
		set => this.value = value;
	}

#if UNITY_EDITOR
	public abstract class Inspector : Editor
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			EditorGUIUtility.labelWidth = 0;
			EditorGUIUtility.fieldWidth = 0;

			var context = target as GenericReference<V>;
			EditorGUI.BeginChangeCheck();

			EditorGUILayout.BeginHorizontal();

			var next = DrawField(context.name, context.Value);
			EditorGUILayout.EndHorizontal();

			if(EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(context, $"{context} - Changed Value");
				EditorUtility.SetDirty(context);
				context.Value = next;
			}
			serializedObject.ApplyModifiedProperties();
		}

		public abstract V DrawField(string label, V current);
	}
#endif
}
