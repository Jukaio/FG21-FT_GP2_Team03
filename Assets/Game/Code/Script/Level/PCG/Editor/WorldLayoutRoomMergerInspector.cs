using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Freya;
using UnityEngine.InputSystem;

namespace Levels
{
	[CustomEditor(typeof(WorldLayoutRoomMerger))]
	public class WorldLayoutRoomMergerInspector : WorldLayoutInspectorBase
	{
		private List<Room> rooms = null;
		private Color[] debugColours = null;
		private Room[,] layout = null;
		public override void OnInspectorGUI()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUI.color = Color.white;
			if(GUILayout.Button("Generate Merged Rooms", EditorStyles.toolbarButton)) {

				WorldLayoutRoomMerger roomMerger = target as WorldLayoutRoomMerger;
				_ = roomMerger.Generate(out layout, out rooms);
				debugColours = new Color[rooms.Count];
				var fraction = 1.0f / rooms.Count;
				for(int i = 0; i < debugColours.Length; i++) {
					debugColours[i] = Color.HSVToRGB(fraction * i, 1.0f , 0.5f);
				}
			}

			if(GUILayout.Button("Reset", EditorStyles.toolbarButton)) {
				layout = null;
				rooms?.Clear();
				rooms = null;
			}
			GUILayout.EndHorizontal();
			base.OnInspectorGUI();
		}

		private void OnSceneGUI()
		{
			WorldLayoutRoomMerger roomMerger = target as WorldLayoutRoomMerger;
			var settings = roomMerger.Settings;

			// Draw Big Rooms
			DebugDrawLayout(layout, settings);
			DebugDrawRooms(rooms, settings);
			DebugDrawHoverInfo(layout, settings);

		}
	}
}

