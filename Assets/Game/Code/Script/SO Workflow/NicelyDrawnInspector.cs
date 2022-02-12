// Made by David Naußed
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace Utility
{
	/*	By default, applies itself to most Objects
	 *	If you wanna use it for your own, inherit from it instead of Editor and call base.OnInspectorGUI()
	 *	Features:
	 *	*	Collapsable UnityEngine.Object editors
	 *	*	Undoing/Redoing - Except for asset creation and destruction
	 *	*	Title/Label name formatting
	 *	*	Contextual asset creation on null references in inspector
	 *	*	Drawing member fields that are not getting considered by the NicelyDrawnInspector
	 *	*	"Plug and play"
	 *	*	Colour-coded inspectors for visual support
	 *	*	Tooltips
	 *	*	Capabilities to use it for your own Inspectors
	 *	*	Saving :) */
	// TODO: Multiple Object editing
	[CustomEditor(typeof(Object), true)]
	//[CanEditMultipleObjects]
	public class NicelyDrawnInspector : Editor
	{
		private FieldInfo[] fields = null;
		private Dictionary<FieldInfo, bool> isFolded = new Dictionary<FieldInfo, bool>();
		private Editor[] subEditors = null;
		private string[] exclusion = null;
		private GUIStyle buttonStyle = null;
		private GUIStyle centeredBoldLabel = null;
		private GUIStyle tooltipLabel = null;
		private Color color;

		private void SetStandard() => GUI.color = Color.white;

		private void Initialise()
		{
			CreateLabels();

			CollectAndFitlerData();
		}

		private void CollectAndFitlerData()
		{
			const BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
			fields = target.GetType().GetFields(flags).Where(info => typeof(Object).IsAssignableFrom(info.FieldType)).ToArray();
			fields = fields.Where(info =>
			{
				var serializeFIeld = info.GetCustomAttribute(typeof(SerializeField)) as SerializeField;
				return serializeFIeld != null || info.IsPublic;
			}).ToArray();

			if(subEditors == null) {
				subEditors = new Editor[fields.Length];
			}

			exclusion = new string[fields.Length + 1];
			exclusion[0] = "m_Script";

			for(int i = 1; i < exclusion.Length; i++) {
				exclusion[i] = fields[i - 1].Name;
			}
		}

		private void CreateLabels()
		{
			color = Color.HSVToRGB(Random.Range(0.0f, 1.0f), 0.5f, 1.0f);
			buttonStyle = new GUIStyle(EditorStyles.toolbarButton);
			buttonStyle.fontStyle = FontStyle.Bold;
			buttonStyle.wordWrap = true;

			centeredBoldLabel = new GUIStyle(EditorStyles.boldLabel);
			centeredBoldLabel.alignment = TextAnchor.MiddleCenter;

			tooltipLabel = new GUIStyle(EditorStyles.miniLabel);
			tooltipLabel.alignment = TextAnchor.UpperCenter;
			tooltipLabel.wordWrap = true;
		}

		public override void OnInspectorGUI()
		{
			if(fields == null) {
				Initialise(); // Just in case, cause OnEnable threw bugs >:(
			}

			serializedObject.Update();
			for(var i = 0; i < fields.Length; i++) {
				var field = fields[i];
				EditorGUI.BeginChangeCheck();

				GUI.color = !(isFolded.TryGetValue(field, out bool fold) && fold) ? Color.white : color;
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				ForEachField(i, field);

				EditorGUILayout.Space();
				EditorGUILayout.EndVertical();
				EditorGUILayout.Space();
			}
			DrawPropertiesExcluding(serializedObject, exclusion);
			serializedObject.ApplyModifiedProperties();
		}

		private void ForEachField(int i, FieldInfo field)
		{
			var previous = field.GetValue(target) as Object;
			var formattedTitle = Formatter.UnityInspectorTitle(field.Name);
			GUIContent content = new GUIContent(formattedTitle);
			if(GUILayout.Button(content, buttonStyle, GUILayout.Height(buttonStyle.CalcSize(content).y))) {
				isFolded[field] = !(isFolded.TryGetValue(field, out bool fol) && fol);
				if(isFolded[field] == false) {
					subEditors[i] = null;
				}
			}
			SetStandard();

			DrawTooltip(field);

			EditorGUILayout.BeginHorizontal(EditorStyles.helpBox);
			var current = EditorGUILayout.ObjectField
			(
				previous,
				field.FieldType,
				!EditorUtility.IsPersistent(target)
			) as Object;

			if(current == null) {
				current = TryDrawAssetCreateButton(field, formattedTitle, current);
			}
			if(isFolded.TryGetValue(field, out bool folded) && folded) {
				if((previous == null && current != null)
					|| (isFolded[field] && subEditors[i] == null)) {
					subEditors[i] = CreateEditor(current);
				}
			}

			EditorGUILayout.EndHorizontal();
			subEditors[i]?.OnInspectorGUI();
			if(EditorGUI.EndChangeCheck()) {
				Undo.RecordObject(target, $"{current} changed");
				field.SetValue(target, current);
				EditorUtility.SetDirty(target);
			}
		}

		public Object TryDrawAssetCreateButton(FieldInfo field, string formattedTitle, Object current)
		{
			if(typeof(ScriptableObject).IsAssignableFrom(field.FieldType) && GUILayout.Button("Create")) {
				var instance = ScriptableObject.CreateInstance(field.FieldType);
				var path = EditorUtility.IsPersistent(target) ? 
					AssetDatabase.GetAssetPath(target).Replace(".asset", $" {formattedTitle}.asset") :
					$"Assets/{formattedTitle}.asset";
				AssetDatabase.CreateAsset
				(
					instance,
					path
				);
				Debug.Log(path);
				EditorUtility.SetDirty(target);
				current = instance;
			}

			return current;
		}

		private void DrawTooltip(FieldInfo field)
		{
			var tooltip = field.GetCustomAttribute(typeof(TooltipAttribute)) as TooltipAttribute;
			if(tooltip != null) {
				EditorGUILayout.LabelField(tooltip.tooltip, tooltipLabel);
			}
		}
	}

	public static class Formatter
	{
		public static string UnityInspectorTitle(string name)
		{
			System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(@"^_|^._");
			string fieldName = regex.Replace(name, @"");
			fieldName = System.Text.RegularExpressions.Regex.Replace(fieldName, @"((?<=\p{Ll})\p{Lu})|((?!\A)\p{Lu}(?>\p{Ll}))", " $0");
			fieldName = System.Text.RegularExpressions.Regex.Replace(fieldName, @"^ {1,}", "");
			return System.Text.RegularExpressions.Regex.Replace(fieldName, @"^\w", m => m.Value.ToUpper());
		}
	}
}
#endif
