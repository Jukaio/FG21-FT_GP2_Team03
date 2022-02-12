using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using Freya;
using System.Collections.Generic;

namespace Levels
{
	public abstract class WorldLayoutInspectorBase : Utility.NicelyDrawnInspector
	{
		protected static void DebugDrawLayout(Room[,] layout, WorldLayoutSettings settings)
		{
			if(layout == null || settings == null) {
				return;
			}
			var min = settings.Rect.min;
			var max = settings.Rect.max;

			for(int x = min.x; x < max.x; x++) {
				for(int z = min.y; z < max.y; z++) {
					var index = new Vector2Int(x, z) - min;
					var position = new Vector3(x, 0, z);

					Handles.color = layout[index.x, index.y] != null ? Color.green : Color.black;
					position = Vector3.Scale(position, new Vector3(settings.RoomSize.x, 1.0f, settings.RoomSize.y));
					position += settings.RoomSize.X0Y() * 0.5f;
					Handles.DrawWireCube
					(
						position,
						(settings.RoomSize.X0Y() - (settings.WallThickness.X0Y() * 2) + Vector3.up)
					);
				}
			}
		}

		protected static void DebugDrawHoverInfo(Room[,] layout, WorldLayoutSettings settings)
		{
			if(layout == null || settings == null) {
				return;
			}

			Vector3 mousePosition = Event.current.mousePosition;
			Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
			Plane plane = new Plane(Vector3.up, Vector3.zero);
			if(plane.Raycast(ray, out float distance)) {
				var endPoint = ray.origin + (ray.direction * distance);
				var index = settings.WorldToLayoutIndex(endPoint);
				if(settings.InBounds(index)) {
					var world = settings.LayoutIndexToWorld(index);
					Handles.color = Color.white;
					Handles.DrawWireCube(world, (settings.RoomSize.X0Y() * 0.5f) + Vector3.up);
					index = settings.LayoutIndexToTwoDimensionalArrayIndex(index.x, index.y);

					var selection = layout[index.x, index.y];
					if(selection != null) {
						Handles.color = Color.yellow;
						foreach(var neighbour in selection.Neighbours) {
							foreach(var neighbourIndex in neighbour.Indeces) {
								var position = settings.LayoutIndexToWorld(neighbourIndex);
								Handles.DrawWireCube
								(
									position,
									(settings.RoomSize.X0Y() * 0.5f - (settings.WallThickness.X0Y() * 2) + Vector3.up) + Vector3.up
								);
							}
						}
						Handles.color = Color.magenta;
						foreach(var memberIndex in selection.Indeces) {
							var position = settings.LayoutIndexToWorld(memberIndex);
							Handles.DrawWireCube
							(
								position,
								(settings.RoomSize.X0Y() * 0.5f - (settings.WallThickness.X0Y() * 2) + Vector3.up) + Vector3.up
							);
						}
					}
				}
			}
		}

		protected static void DebugDrawRooms(List<Room> rooms, WorldLayoutSettings settings)
		{
			if(rooms == null || settings == null) {
				return;
			}
			for(var ii = 0; ii < rooms.Count; ii++) {
				var room = rooms[ii];
				for(var i = 0; i < room.Indeces.Count; i++) {
					var memberIndex = room.Indeces[i];
					var index = settings.TwoDimensionalArrayIndexToLayoutIndex(memberIndex.x, memberIndex.y);

					var position = new Vector3(index.x, 0.0f, index.y);
					position = Vector3.Scale(position, new Vector3(settings.RoomSize.x, 1.0f, settings.RoomSize.y));
					position += settings.RoomSize.X0Y() * 0.5f;

					Handles.color = Color.cyan;
					Handles.DrawWireCube
					(
						position,
						(settings.RoomSize.X0Y() - (settings.WallThickness.X0Y() * 2) + Vector3.up)
					);

					Handles.Label(position, $"{ii}");
				}
			}
		}

	}

	[CustomEditor(typeof(WorldLayout))]
	public class WorldLayoutInspector : WorldLayoutInspectorBase
	{
		BoxBoundsHandle boxHandle = new BoxBoundsHandle();
		Room[,] layout = null;

		public override void OnInspectorGUI()
		{
			GUILayout.BeginHorizontal(EditorStyles.toolbar);
			GUI.color = Color.white;	
			WorldLayout worldLayout = target as WorldLayout;
			if(GUILayout.Button("Generate Layout", EditorStyles.toolbarButton)) {

				worldLayout.Generate(worldLayout.Center, out layout, out var _);
			}
			if(GUILayout.Button("Reset", EditorStyles.toolbarButton)) {
				layout = null;
			}

			GUILayout.EndHorizontal();
			base.OnInspectorGUI();
		}

		private void OnSceneGUI()
		{
			WorldLayout worldLayout = target as WorldLayout;

			DrawWorldLayoutHandles(worldLayout);
			DebugDrawLayout(layout, worldLayout.Settings);
		}

		private void DrawWorldLayoutHandles(WorldLayout worldLayout)
		{
			Vector2 center = worldLayout.Rect.center;
			Vector2 size = worldLayout.Rect.size;

			center.Scale(worldLayout.RoomSize);
			size.Scale(worldLayout.RoomSize);

			boxHandle.center = center.X0Y();
			boxHandle.size = size.X0Y();
			boxHandle.wireframeColor = Color.red;

			EditorGUI.BeginChangeCheck();
			boxHandle.DrawHandle();
			if(EditorGUI.EndChangeCheck()) {
				layout = null;
				Undo.RecordObject(worldLayout, "Change Bounds");
				RectInt nextRect = new RectInt();

				var newSize = boxHandle.size.XZ();
				var newCenter = boxHandle.center.XZ();

				newSize = new Vector2(newSize.x / worldLayout.RoomSize.x, newSize.y / worldLayout.RoomSize.y);
				newCenter = new Vector2(newCenter.x / worldLayout.RoomSize.x, newCenter.y / worldLayout.RoomSize.y);

				nextRect.SetMinMax
				(
					Vector2Int.RoundToInt(newCenter - (newSize / 2.0f)),
					Vector2Int.RoundToInt(newCenter + (newSize / 2.0f))
				);
				var field = worldLayout.Settings.GetType().GetField
				(
					"rect",
					System.Reflection.BindingFlags.NonPublic |
					System.Reflection.BindingFlags.Instance
				);
				field.SetValue(worldLayout.Settings, nextRect);
			}
		}
	}
}

