using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Freya;

namespace Levels
{
	[CustomEditor(typeof(WorldLayoutBuilder))]
	public class WorldLayoutWallBuilderInspector : WorldLayoutInspectorBase
	{
		public override void OnInspectorGUI()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUI.color = Color.white;
			if(GUILayout.Button("Build", EditorStyles.toolbarButton)) {

				WorldLayoutBuilder builder = target as WorldLayoutBuilder;
				builder.Build();
			}

			if(GUILayout.Button("Destroy", EditorStyles.toolbarButton)) {
				WorldLayoutBuilder builder = target as WorldLayoutBuilder;
				builder.Destroy();
			}
			GUILayout.EndHorizontal();
			base.OnInspectorGUI();
		}

		private void OnSceneGUI()
		{

		}
	}
}
