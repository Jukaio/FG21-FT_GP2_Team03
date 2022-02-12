using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using Freya;

namespace Levels
{
	[CustomEditor(typeof(WorldLayoutWallCalculator))]
	public class WorldLayoutWallCalculatorInspector : WorldLayoutInspectorBase
	{
		private Room[,] layout;
		private List<Room> rooms;

		public override void OnInspectorGUI()
		{
			WorldLayoutWallCalculator wallCalculator = target as WorldLayoutWallCalculator;
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUI.color = Color.white;
			if(GUILayout.Button("Generate, Calculate and Preview", EditorStyles.toolbarButton)) {
				if(wallCalculator.Generate(out layout, out rooms)) {
				}
			}

			if(GUILayout.Button("Recalculate", EditorStyles.toolbarButton)) {
				wallCalculator.Calculate(layout, rooms);
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
			// Draw Big Rooms
			WorldLayoutWallCalculator wallCalculator = target as WorldLayoutWallCalculator;
			var settings = wallCalculator.Settings;

			//DebugDrawLayout(layout, settings);
			//DebugDrawRooms(rooms, settings);
			DebugDrawHoverInfo(layout, settings);

			var style = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
			style.alignment = TextAnchor.MiddleCenter;
			style.normal.textColor = Color.white;

			if(rooms == null) {
				return;
			}

			foreach(var room in rooms) {
				foreach(var wall in room.Walls) {
					Handles.color = Color.green;
					Handles.DrawWireCube(wall.Position, settings.WallThickness.X0Y());
					var pos = wall.Position + Vector3.Scale(wall.Normal, settings.WallThickness.X0Y());

					Handles.color = Color.blue;
					Handles.DrawLine(wall.Position, pos, 2.0f);

					if(settings.ShowDebugIndeces) {
						Handles.Label(wall.Position, $"{wall.Index}", style);
					}
				}

				Handles.color = Color.yellow;
				foreach(var wall in room.Corners) {
					Handles.color = Color.yellow;
					Handles.DrawWireCube(wall.Position, settings.WallThickness.X0Y());
					var pos = wall.Position + Vector3.Scale(wall.Normal, settings.WallThickness.X0Y());

					Handles.color = Color.blue;
					Handles.DrawLine(wall.Position, pos, 2.0f);

					if(settings.ShowDebugIndeces) {
						Handles.Label(wall.Position, $"{wall.Index}", style);
					}
				}

				Handles.color = Color.magenta;
				foreach(var wall in room.Doors) {
					Handles.color = Color.magenta;
					Handles.DrawWireCube(wall.Position, settings.WallThickness.X0Y());
					var pos = wall.Position + Vector3.Scale(wall.Normal, settings.WallThickness.X0Y());

					Handles.color = Color.blue;
					Handles.DrawLine(wall.Position, pos, 2.0f);

					if(settings.ShowDebugIndeces) {
						Handles.Label(wall.Position, $"{wall.Index}", style);
					}
				}
			}
		}

	}
}
