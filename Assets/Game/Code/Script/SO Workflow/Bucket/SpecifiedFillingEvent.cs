using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif
namespace Game
{
	[System.Serializable]
	[CreateAssetMenu(fileName = "Specified Filling Int Event", menuName = "Scriptable Objects/Gameplay/Bucket/Specified Filling")]
	public class SpecifiedFillingEvent : ScriptableObject
	{
		[SerializeField]
		private float min;
		[SerializeField]
		private float max;
		[SerializeField]
		private FloatEvent onSpecifiedFilling;

		public float Min => min;
		public float Max => max;
		public event System.Action<float> OnSpecifiedFilling
		{
			add => onSpecifiedFilling.onEvent += value;
			remove => onSpecifiedFilling.onEvent += value;
		}

		public void Invoke(float current)
		{
			onSpecifiedFilling?.Invoke(current);
		}
	}


#if UNITY_EDITOR
	[CustomEditor(typeof(SpecifiedFillingEvent), true)]
	public class SpecifiedFillingEventInspector : Utility.NicelyDrawnInspector
	{
		public override void OnInspectorGUI()
		{
			serializedObject.Update();
			BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
			var context = target as SpecifiedFillingEvent;
			var type = context.GetType();
			var fillingEventField = type.GetField("onSpecifiedFilling", flags);

			EditorGUI.BeginChangeCheck();
			EditorGUILayout.BeginHorizontal();
			var next = EditorGUILayout.ObjectField
			(
				"Specified Filling Int Event", 
				fillingEventField.GetValue(context) as ScriptableObject, 
				fillingEventField.FieldType, 
				false
			);
			next = TryDrawAssetCreateButton(fillingEventField, "Specified Filling Int Event", next);
			EditorGUILayout.EndHorizontal();
			if(EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(context, $"Undo Specified Int Event Assignment");
				fillingEventField.SetValue(context, next);
				EditorUtility.SetDirty(context);
			}

			if(next == null) {
				return;			
			}

			var minField = type.GetField("min", flags);
			var maxField = type.GetField("max", flags);

			GUIStyle style = new GUIStyle(EditorStyles.boldLabel);
			style.alignment = TextAnchor.MiddleCenter;

			EditorGUILayout.LabelField("Range", style);
			EditorGUILayout.BeginHorizontal();

			EditorGUI.BeginChangeCheck();
			var minValue = EditorGUILayout.FloatField((float)minField.GetValue(context));
			var maxValue = (float)maxField.GetValue(context);
			EditorGUILayout.MinMaxSlider(ref minValue, ref maxValue, 0, 1.0f);
			maxValue = EditorGUILayout.FloatField(maxValue);
			if(EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(context, $"Undo min-max range changes");
				fillingEventField.SetValue(context, next);
				
				minField.SetValue(context, minValue);
				maxField.SetValue(context, maxValue);
				EditorUtility.SetDirty(context);
			}

			EditorGUILayout.EndHorizontal();
			serializedObject.ApplyModifiedProperties();
		}
	}
#endif
}
