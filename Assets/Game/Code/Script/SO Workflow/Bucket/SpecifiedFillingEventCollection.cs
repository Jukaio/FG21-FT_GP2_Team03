using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using System.Reflection;
#endif
namespace Game
{
	[CreateAssetMenu(fileName = "Specified Filling Int Event Collection", menuName = "Scriptable Objects/Gameplay/Bucket/Specified Filling Collection")]
	public class SpecifiedFillingEventCollection : ScriptableObject
	{
		[SerializeField]
		private List<SpecifiedFillingEvent> events = new List<SpecifiedFillingEvent>();

		public List<SpecifiedFillingEvent> Events => events;
	}

#if UNITY_EDITOR
	[CustomEditor(typeof(SpecifiedFillingEventCollection), true)]
	public class SpecifiedFillingEventCollectionInspector : Editor
	{
		public override void OnInspectorGUI()
		{
			BindingFlags flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic;
			var context = target as SpecifiedFillingEventCollection;

			var type = context.GetType();
			var fillingEventField = type.GetField("events", flags);

			var fillingEventCollection = fillingEventField.GetValue(context) as List<SpecifiedFillingEvent>;

			for(var i = 0; i < fillingEventCollection.Count; i++) {
				var element = fillingEventCollection[i];

				EditorGUILayout.BeginHorizontal();
				EditorGUI.BeginChangeCheck();
				var next = EditorGUILayout.ObjectField(element, typeof(SpecifiedFillingEvent), false) as SpecifiedFillingEvent;
				if(EditorGUI.EndChangeCheck()) {
					Undo.RecordObject(context, $"Added new Item to SpecifiedFillingEventCollection");
					fillingEventCollection[i] = next;
					
					break;
				}

				if(next == null) {
					if(GUILayout.Button("Create")) {
						var formattedTitle = Utility.Formatter.UnityInspectorTitle($"Level {i}");
						var instance = CreateInstance(typeof(SpecifiedFillingEvent)) as SpecifiedFillingEvent;
						AssetDatabase.CreateAsset
						(
							instance,
							AssetDatabase.GetAssetPath(context).Replace(".asset", $" {formattedTitle}.asset")
						);
						fillingEventCollection[i] = instance;
						EditorUtility.SetDirty(context);
						break;
					}
				}

				if(GUILayout.Button("Remove")) {
					fillingEventCollection.RemoveAt(i);
					EditorUtility.SetDirty(context);
					break;
				}
				EditorGUILayout.EndHorizontal();
			}
			if(GUILayout.Button("Add")) {
				fillingEventCollection.Add(null);
				EditorUtility.SetDirty(context);
			}

			EditorGUILayout.Space();

			var centered = EditorStyles.boldLabel;
			centered.alignment = TextAnchor.MiddleCenter;
			centered.wordWrap = true;
			foreach(var fillingEvent in fillingEventCollection) {
				EditorGUILayout.BeginVertical(EditorStyles.helpBox);
				if(fillingEvent != null) {

					EditorGUILayout.LabelField($"{fillingEvent.name}\n{fillingEvent.Min} - {fillingEvent.Max}", centered);
					CreateEditor(fillingEvent)?.OnInspectorGUI();

				}
				else {
					EditorGUILayout.LabelField($"NULL", centered);
				}

				EditorGUILayout.Space();
				EditorGUILayout.EndVertical();

				EditorGUILayout.Space();
			}
		}
	}
#endif
}
