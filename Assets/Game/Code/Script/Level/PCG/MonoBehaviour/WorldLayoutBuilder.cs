using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Levels
{
	[RequireComponent(typeof(WorldLayout), typeof(WorldLayoutRoomMerger), typeof(WorldLayoutWallCalculator))]
	public class WorldLayoutBuilder : MonoBehaviour
	{
		[SerializeField]
		private WorldBuilderSettings settings;

		private WorldLayoutWallCalculator cachedWallCalculator;

		public WorldLayoutWallCalculator WallCalculator
		{
			get
			{
				if(cachedWallCalculator == null) {
					cachedWallCalculator = GetComponent<WorldLayoutWallCalculator>();
				}
				return cachedWallCalculator;
			}
		}
		public WorldLayoutSettings LayoutSettings => WallCalculator.Settings;
		private WorldRoom[,] worldRoomLayout = null;
		private List<WorldRoom> worldRooms = new List<WorldRoom>();

		public WorldRoom[,] WorldRoomLayout => worldRoomLayout;
		public List<WorldRoom> WorldRooms => worldRooms;
		public void Build()
		{
			if(worldRooms.Count > 0) {
				Destroy();
			}
			if(WallCalculator.Generate(out var layout, out var rooms)) {
				worldRoomLayout = new WorldRoom[layout.GetLength(0), layout.GetLength(1)];

				// Measure
				var furthestRoom = settings.HasFinalRoom ? FindFurthestRoom(rooms) : null;

				if(settings.RoomPrefab != null) {
					foreach(var room in rooms) {
						InstantiateWorldRoomWithGeneratedRoomData(room, room == furthestRoom);
					}
				}
				
			}
		}

		private void InstantiateWorldRoomWithGeneratedRoomData(Room room, bool isFinalRoom)
		{
			var worldRoom = WorldRoom.CreateFromRoom(worldRoomLayout, room, settings, isFinalRoom);
			worldRoom.transform.SetParent(transform, true);
			worldRooms.Add(worldRoom);
			worldRoom.gameObject.SetActive(false);
			worldRoom.gameObject.isStatic = true;
		}

		private Room FindFurthestRoom(List<Room> rooms)
		{
			Room furthestRoom = null;
			float distance = float.MinValue;
			foreach(var room in rooms) {
				var delta = Vector2Int.Distance(settings.LayoutSettings.Center, room.Indeces[0]);
				if(delta > distance) {
					distance = delta;
					furthestRoom = room;
				}
			}
			return furthestRoom;
		}

		public void Destroy()
		{
			foreach(var go in worldRooms) {
				if(go != null) {
					DestroyImmediate(go.gameObject);
				}
			}
			worldRooms.Clear();
		}
	}
}


